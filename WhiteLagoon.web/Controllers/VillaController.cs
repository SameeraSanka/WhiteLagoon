using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var villa = _db.Villas.ToList();
            return View(villa);
        }


        public IActionResult Create()
        {
            return View();
        }

        //Add
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if(obj.Name == obj.Description)
            {
                ModelState.AddModelError("name", "The Description cannot exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.Villas.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }

        //view selected villa
        [HttpGet]
        public IActionResult Update(int villaId)
        {
            Villa? villa = _db.Villas.FirstOrDefault(villa => villa.Id == villaId);

            // mee widihatath dat  ganna puluwan conditon wlata
            //Villa? villa = _db.Villas.Find(villaId);
            //var VillaList = _db.Villas.Where(villa=> villa.Price == villaId && villa.Occupancy>0);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        //update selected villa
        [HttpPost]
        public IActionResult Update(Villa obj)
        {

            // && obj.Id>0 me part eken wenne Id eka 0 unth aluth recode ekak widihata db ekata
            // yna eka nwaththanna. update ekedi kohomth Id ekak ewnne. eka thma methanin balanne
            if (ModelState.IsValid && obj.Id>0)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int villaId)
        {
            Villa? villa = _db.Villas.FirstOrDefault(villa => villa.Id == villaId);

            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villa = _db.Villas.FirstOrDefault(villa => villa.Id == obj.Id);
            if (villa is not null)
            {
                _db.Villas.Remove(villa);
                _db.SaveChanges();
                return RedirectToAction("Index", "Villa");
            }
            return View();
        }
    }
}
