using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBack.Areas.Identity.Data;
using WebBack.Models;

namespace WebBack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Category category)
        {
            if(ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet("/Categories/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if(id<=0) return BadRequest();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost("/Categories/Edit/{id}")]
        public IActionResult Edit(int id, Category cat)
        {
            if(ModelState.IsValid)
            {
                var category = new Category
                {
                    Id = cat.Id,
                    Name = cat.Name
                };
                // _context.Categories.Update(category);
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(cat);
        }
        public ActionResult Details(int id)
        {
            // AppInfo();
            var categories = _context.Categories.FirstOrDefault(z => z.Id == id);
            if (id<=0 || categories==null)
            {
                return BadRequest();
            }
            else if(!_context.Categories.Any(x => x.Id == id))
            {
                return NotFound();
            }
            return View(categories);
        }
    }
}