using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            // u=> meken kiyawenne it's like saying "for each villa (u) in the list, do the following:"
            // ekiynne collection eke thiyna okkoma element text ekai value ekai illana widita map kranna kiyla
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u=> new SelectListItem
            {
                Text= u.Name,
                Value=u.Id.ToString(),
            });
            //me view data function eken thmai uda gaththa list eka view ekata ywnne. return eken ywanna ba. 
            // ewagema thawa ViewBag kiyla ekak thiynwa ekenuth krnne temparaly data controller eken view ekata data aran yna eka.
            // ViewBag.list = list;
            // ViewData kiynne type of data dictionary ekak.eth view bag dynamic type property ekak
            ViewData["list"] = list;
             return View();
        }

        //Add villa number
        [HttpPost]
        public IActionResult Create(VillaNumber obj)
        {
            if(ModelState.IsValid)
            {
                _db.VillaNumbers.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa Number has been Added successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
