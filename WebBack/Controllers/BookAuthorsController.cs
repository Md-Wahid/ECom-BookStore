using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBack.Areas.Identity.Data;

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
    }
}