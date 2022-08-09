using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models.ViewModels;
using Rocky_Utility;
using Rocky_Utility.BrainTree;

namespace Rocky.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderHeaderRepository _ordHRepo;
        private readonly IOrderDetailRepository _ordDRepo;
        private readonly IBrainTreeGate _brain;

        public OrderController(
            IOrderHeaderRepository ordHRepo,
            IOrderDetailRepository ordDRepo,
            IBrainTreeGate brain
            )
        {

            _ordHRepo = ordHRepo;
            _ordDRepo = ordDRepo;
            _brain = brain;

        }

        public IActionResult Index()
        {
            OrderListVM orderListVM = new OrderListVM()
            {
                OrderHList = _ordHRepo.GetAll(),
                StatusList = WC.listStatus.ToList().Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = x,
                    Value = x
                })
            };
            return View(orderListVM);
        }

    }
}