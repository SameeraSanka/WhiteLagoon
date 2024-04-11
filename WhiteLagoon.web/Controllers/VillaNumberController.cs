using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        /*//Add Villa Number
        [HttpPost]
        public async Task<IActionResult> Create(VillaNumberViewModel viewModel)
        {
            var villaNUmber = new VillaNumber
            {
                Villa_Number = viewModel.Villa_Number,
                VillaId = viewModel.VillaId,
                SpecialDetails = viewModel.SpecialDetails,
            };
            await _db.VillaNumbers.AddAsync(villaNUmber);
            await _db.SaveChangesAsync();
            return View();
        }*/

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
