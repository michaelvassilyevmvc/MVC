using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Rocky_Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Rocky_Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Rocky_Utility;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController
        (
            ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index
        ()
        {
            var objList = await _db.Product.Include(x => x.Category).Include(x => x.ApplicationType).ToListAsync();
            return View(objList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            // IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(x => new SelectListItem
            // {
            //     Text = x.Name,
            //     Value = x.Id.ToString()
            // });

            // ViewData["CategoryDropDown"] = CategoryDropDown;

            // Product product = new Product();

            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    _db.Product.Add(productVM.Product);

                }
                else
                {
                    //Updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);

                    if (files.Count() > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    _db.Product.Update(productVM.Product);
                }

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(productVM);
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = await _db.Product.Include(x => x.Category).Include(x => x.ApplicationType).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _db.Product.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}