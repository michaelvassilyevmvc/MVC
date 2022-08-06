using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocky_DataAccess.Data;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Utility;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _catRepo;

        public CategoryController(ILogger<CategoryController> logger, ICategoryRepository catRepo)
        {
            _logger = logger;
            _catRepo = catRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRepo.GetAll();
            return View(objList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Add(obj);
                _catRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _catRepo.Find(id.GetValueOrDefault());

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _catRepo.Update(obj);
                _catRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _catRepo.Find(id.GetValueOrDefault());

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _catRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _catRepo.Remove(obj);
            _catRepo.Save();
            return RedirectToAction("Index");
        }

    }
}