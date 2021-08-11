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
    public class BookStoresController : Controller
    {
        private readonly DataContext _context;
        public BookStoresController(DataContext context)
        {
            _context = context;
        }

        // GET: BookStore
        public async Task<IActionResult> Index()
        {
            // var bookStores = await _context.BookStores
            //                     .Include(x => x.Books)
            //                     .ToListAsync();
            return View(await _context.BookStores.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(BookStore bookStores)
        {
            if(ModelState.IsValid)
            {
                _context.BookStores.Add(bookStores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookStores);
        }

        [HttpGet("/BookStoresEdit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = _context.BookStores.Find(id);
            if(id<=0) return BadRequest();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost("/BookStoresEdit/{id}")]
        public IActionResult Edit(int id, BookStore bookStr)
        {
            if(ModelState.IsValid)
            {
                var bookStore = new BookStore
                {
                    Id = bookStr.Id,
                    Name = bookStr.Name
                };
                // _context.Categories.Update(category);
                _context.Entry(bookStore).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(bookStr);
        }
        public ActionResult Details(int id)
        {
            // AppInfo();
            var bookStores = _context.BookStores.FirstOrDefault(z => z.Id == id);
            if (id<=0 || bookStores==null)
            {
                return BadRequest();
            }
            else if(!_context.BookStores.Any(x => x.Id == id))
            {
                return NotFound();
            }
            return View(bookStores);
        }
    }
}