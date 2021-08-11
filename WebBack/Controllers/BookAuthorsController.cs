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
    public class BookAuthorsController : Controller
    {
        private readonly DataContext _context;
        public BookAuthorsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BookAuthors.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(BookAuthor bookAuthor)
        {
            if(ModelState.IsValid)
            {
                _context.BookAuthors.Add(bookAuthor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookAuthor);
        }

        [HttpGet("/BookAuthors/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var bookAuthor = _context.BookAuthors.Find(id);
            if(id<=0) return BadRequest();
            if (bookAuthor == null)
            {
                return NotFound();
            }
            return View(bookAuthor);
        }

        [HttpPost("/BookAuthors/Edit/{id}")]
        public IActionResult Edit(int id, Category bookAuth)
        {
            if(ModelState.IsValid)
            {
                var bookAuthor = new Category
                {
                    Id = bookAuth.Id,
                    Name = bookAuth.Name
                };
                // _context.BookAuthors.Update(category);
                _context.Entry(bookAuthor).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(bookAuth);
        }

        public ActionResult Details(int id)
        {
            // AppInfo();
            var authors = _context.BookAuthors.FirstOrDefault(z => z.Id == id);
            if (id<=0 || authors==null)
            {
                return BadRequest();
            }
            else if(!_context.BookAuthors.Any(x => x.Id == id))
            {
                return NotFound();
            }
            return View(authors);
        }
    }
}