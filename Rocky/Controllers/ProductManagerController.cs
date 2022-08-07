using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Rocky_Models;
using Rocky_Utility;

namespace Rocky.Controllers
{
    public class ProductManagerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductManagerController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var list = await _db.ProductManager.ToListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductManager product)
        {
            _db.ProductManager.Add(product);
            await _db.SaveChangesAsync();
            TempData[WC.Success] = "Operation is success!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = await _db.ProductManager.FindAsync(id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductManager product)
        {
            if (ModelState.IsValid)
            {
                _db.ProductManager.Update(product);
                await _db.SaveChangesAsync();
                TempData[WC.Success] = "Operation is success!";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Operation is error!";
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = await _db.ProductManager.FindAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var obj = await _db.ProductManager.FindAsync(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.ProductManager.Remove(obj);
            await _db.SaveChangesAsync();
            TempData[WC.Success] = "Operation is success!";
            return RedirectToAction("Index");
        }
    }
}