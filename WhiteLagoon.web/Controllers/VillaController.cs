using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villa = _unitOfWork.Villa.GetAll();
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
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been Added successfully";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "THe villa cannot be added";
            return View();
        }

        //view selected villa
        [HttpGet]
        public IActionResult Update(int villaId)
        {
            Villa? villa = _unitOfWork.Villa.Get(villa => villa.Id == villaId);

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
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been Updated successfully";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "THe villa cannot be Updated";
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int villaId)
        {
            Villa? villa = _unitOfWork.Villa.Get(villa => villa.Id == villaId);

            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        //delete
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? villa = _unitOfWork.Villa.Get(villa => villa.Id == obj.Id);
            if (villa is not null)
            {
                _unitOfWork.Villa.Remove(villa);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "THe villa cannot be deleted";
            return View();
        }
    }
}
