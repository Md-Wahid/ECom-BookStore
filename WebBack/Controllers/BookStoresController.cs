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
    }
}