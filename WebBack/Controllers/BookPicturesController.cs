using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBack.Areas.Identity.Data;
using WebBack.Models;
using WebBack.ViewModels;

namespace WebBack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookPicturesController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;
        public BookPicturesController(DataContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.BookPictures.ToList());
        }

        public IActionResult Create()
        {
            PopulateBookIdDropDownList();
            PopulateCategoryDropDownList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookPictureViewModel bookPictureViewModel)
        {
            if(ModelState.IsValid)
            {
                string imageUploadPath = Path.Combine(_env.WebRootPath, "uploads/images");
                string fileName = await UploadFileAsync(bookPictureViewModel.Picture);
                string pictureUri = "/uploads/images/" + fileName;
                BookPicture bookPucture = new BookPicture
                {
                    Title = bookPictureViewModel.Title,
                    PictureUri = pictureUri,
                    PictureName = bookPictureViewModel.PictureName,
                    BookId = bookPictureViewModel.BookId
                };
                foreach (var cat in bookPictureViewModel.Categories)
                {
                    
                }
                _context.BookPictures.Add(bookPucture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookPictureViewModel);
        }

        public IActionResult Edit(int id)
        {
            if(id<=0) return BadRequest("Invalid Request");
            if(!_context.BookPictures.Any(p => p.Id == id)) return NotFound("No content available");
            var bookPucture = _context.BookPictures.Find(id);
            var bookPictureViewModel = new BookPictureViewModel
            {
                Title = bookPucture.Title,
                PictureUri = bookPucture.PictureUri,
                PictureName = bookPucture.PictureName
            };
            PopulateBookIdDropDownList(bookPucture.BookId);
            return View(bookPictureViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookPictureViewModel bookPictureViewModel)
        {
            if(ModelState.IsValid)
            {
                string imageUploadPath = Path.Combine(_env.WebRootPath, "uploads/images");
                string fileName = await UploadFileAsync(bookPictureViewModel.Picture);
                string pictureUri = "/uploads/images/" + fileName;
                BookPicture bookPucture = new BookPicture
                {
                    Title = bookPictureViewModel.Title,
                    PictureUri = pictureUri,
                    PictureName = bookPictureViewModel.PictureName,
                    BookId = bookPictureViewModel.BookId
                };
                _context.BookPictures.Add(bookPucture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookPictureViewModel);
        }

        private void PopulateCategoryDropDownList(object selectedCategories = null)
        {
            var categoriesQuery = from b in _context.Categories
                                  orderby b.Name
                                  select b;
            ViewBag.CategoriesId = new SelectList(categoriesQuery.AsNoTracking(), "Id", "Name", selectedCategories);
        }

        private async Task<string> UploadFileAsync(IFormFile file)
        {
            string fileName = string.Empty;
            if (file != null)
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "uploads");
                string imageUploadPath = Path.Combine(_env.WebRootPath, "uploads/images");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                // string filePath = Path.Combine(uploadDir, file.FileName);
                string filePath = Path.Combine(imageUploadPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return  fileName;
        }

        private void PopulateBookIdDropDownList(object selectedBook = null)
        {
            var bookQuery = from a in _context.Books
                                   orderby a.Id
                                   select a;
            ViewBag.BookId = new SelectList(bookQuery.AsNoTracking(), "Id", "Name", selectedBook);
        }
    }
}