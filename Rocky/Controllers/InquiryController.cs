using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Models.ViewModels;
using Rocky_Utility;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailRepository _inqDRepo;
        [BindProperty]
        public InquiryVM InquiryVM { get; set; }

        public InquiryController(IInquiryHeaderRepository inqHRepo, IInquiryDetailRepository inqDRepo)
        {
            _inqHRepo = inqHRepo;
            _inqDRepo = inqDRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM
            {
                InquiryHeader = _inqHRepo.FirstOrDefault(x => x.Id == id),
                InquiryDetail = _inqDRepo.GetAll(x => x.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            InquiryVM.InquiryDetail = _inqDRepo.GetAll(u => u.InquiryHeaderId == InquiryVM.InquiryHeader.Id);

            foreach (var detail in InquiryVM.InquiryDetail)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId
                };
                shoppingCartList.Add(shoppingCart);
            }

            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.InquiryHeader.Id);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inqHRepo.FirstOrDefault(x => x.Id == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inqDRepo.GetAll(x => x.InquiryHeaderId == InquiryVM.InquiryHeader.Id);
            _inqDRepo.RemoveRange(inquiryDetails);
            _inqHRepo.Remove(inquiryHeader);
            _inqHRepo.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inqHRepo.GetAll() });
        }
        #endregion
    }
}