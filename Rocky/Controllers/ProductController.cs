using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Microsoft.EntityFrameworkCore;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ProductController: Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(){
            var list = await _db.Product.ToListAsync();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product){
            _db.Product.Add(product);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}