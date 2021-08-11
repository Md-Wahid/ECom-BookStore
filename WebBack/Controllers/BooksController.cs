using System;
using System.Collections.Generic;
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
    public class BooksController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _env;
        public BooksController(DataContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

        // [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateBookAuthorDropDownList();
            PopulateBookStoreDropDownList();
            PopulateCategoryDropDownList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {

                // long size = bookViewModel.Pictures.Sum(f => f.Length);
                // string fileName;
                // // int i = 0;

                // foreach (var formFile in bookViewModel.Pictures)
                // {
                //     // fileName = UploadFile(formFile);
                // string imageUploadPath = Path.Combine(_env.WebRootPath, "uploads/images");
                //     fileName = await UploadFileAsync(formFile);
                //     // fileNames[i] = GetUniqueFileName(formFile.FileName);
                //     // if (formFile.Length > 0)
                //     // {
                //     //     // var filePath = Path.GetTempFileName();
                //     //     var filePath = Path.GetTempFileName();

                //     //     using (var stream = System.IO.File.Create(filePath))
                //     //     {
                //     //         await formFile.CopyToAsync(stream);
                //     //         bookViewModel.Name = filePath;
                //     //     }
                //     // }
                //     // ++i;
                //             bookViewModel.Name = fileName;
                // }
                //             return View(bookViewModel);

                if(bookViewModel.Categories.Count == 0)
                {
                    return View(bookViewModel);
                }

                var book = new Book
                {
                    Id = GetUniqueBookId(),
                    Name = bookViewModel.Name,
                    Description = bookViewModel.Description,
                    ISBN = bookViewModel.ISBN,
                    Price = bookViewModel.Price,
                    // BookPictures = bookViewModel.Pictures,
                    BookAuthorId = bookViewModel.BookAuthorId,
                    BookStoreId = bookViewModel.BookStoreId
                };
                
                foreach (var picture in bookViewModel.Pictures)
                {
                    book.BookPictures.Add(new BookPicture()
                    {
                        // PictureUri = uploadImage(picture),
                        PictureUri = "/uploads/images/",
                        PictureName = await UploadFileAsync(picture),
                        Title = book.Name,
                        Book = book
                    });
                }
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            // AppInfo();
            // Book purchase = _context.Sales.Where(x => x.SaleId == id).Include(prod => prod.SoldProduct).OrderByDescending(x => x.SaleId).FirstOrDefault(z => z.SaleId == id);
            var book = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookStore).Include(b => b.BookPictures).FirstOrDefault(z => z.Id == id);
            if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            else if(!_context.Books.Any(x => x.Id == id) || book == null)
            {
                return NotFound();
            }
            var bookViewModel = new BookViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Description = book.Description,
                ISBN = book.ISBN,
                Price = book.Price,
                BookAuthorId = book.BookAuthorId,
                BookStoreId = book.BookStoreId
            };
            // return View(books);
            PopulateBookAuthorDropDownList(book.BookAuthorId);
            PopulateBookStoreDropDownList(book.BookStoreId);
            PopulateCategoryDropDownList();
            return View(bookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Id = GetUniqueBookId(),
                    Name = bookViewModel.Name,
                    Description = bookViewModel.Description,
                    ISBN = bookViewModel.ISBN,
                    Price = bookViewModel.Price,
                    BookAuthorId = bookViewModel.BookAuthorId,
                    BookStoreId = bookViewModel.BookStoreId
                };
                
                foreach (var picture in bookViewModel.Pictures)
                {
                    book.BookPictures.Add(new BookPicture()
                    {
                        // PictureUri = uploadImage(picture),
                        PictureUri = "/uploads/images",
                        PictureName = await UploadFileAsync(picture),
                        Book = book
                    });
                }
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        public ActionResult Details(string id)
        {
            // AppInfo();
            var books = _context.Books.Include(b => b.BookAuthor).Include(b => b.BookStore).Include(b => b.BookPictures).FirstOrDefault(z => z.Id == id);
            // Book purchase = _context.Sales.Where(x => x.SaleId == id).Include(prod => prod.SoldProduct).OrderByDescending(x => x.SaleId).FirstOrDefault(z => z.SaleId == id);
            if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            else if(!_context.Books.Any(x => x.Id == id) || books == null)
            {
                return NotFound();
            }
            return View(books);
        }

        private string GetUniqueBookId()
        {
            var id = System.Guid.NewGuid().ToString();
            while(_context.Books.Any(x => x.Id == id))
            {
                id = System.Guid.NewGuid().ToString();
            }

            return id;
        }

        private void PopulateBookAuthorDropDownList(object selectedBookAuthors = null)
        {
            var bookAuthorsQuery = from a in _context.BookAuthors
                                   orderby a.Name
                                   select a;
            ViewBag.BookAuthorId = new SelectList(bookAuthorsQuery.AsNoTracking(), "Id", "Name", selectedBookAuthors);
        }

        private void PopulateBookStoreDropDownList(object selectedBookStores = null)
        {
            var bookStoresQuery = from b in _context.BookStores
                                  orderby b.Name
                                  select b;
            ViewBag.BookStoreId = new SelectList(bookStoresQuery.AsNoTracking(), "Id", "Name", selectedBookStores);
        }

        private void PopulateCategoryDropDownList(object selectedCategories = null)
        {
            var categoriesQuery = from b in _context.Categories
                                  orderby b.Name
                                  select b;
            ViewBag.CategoriesId = new SelectList(categoriesQuery.AsNoTracking(), "Id", "Name", selectedCategories);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                    + "_"
                    + Guid.NewGuid().ToString().Substring(0, 4)
                    + Path.GetExtension(fileName);
        }
        private string UploadFile(IFormFile file)
        {
            string fileName = string.Empty;
            if (file != null)
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "uploads");
                string imageUploadPath = Path.Combine(_env.WebRootPath, "uploads/images");
                // string imageUploadPath, videoUploadPath, documentUploadPath;
                // string imageUploadPath;
                // imageUploadPath = Path.Combine(uploadDir, "images");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return  fileName;
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
            return fileName;
        }

        private string uploadImage(IFormFile picture)
        {
            string fileName = null;
            if(picture != null)
            {
                string uploadDir = Path.Combine(_env.WebRootPath, "Images");
                fileName = Guid.NewGuid().ToString() + "-" +picture.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    picture.CopyTo(fileStream);
                }
            }
            return fileName;
        }
    }
}