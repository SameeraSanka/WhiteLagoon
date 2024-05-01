using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.web.ViewModels;

namespace WhiteLagoon.web.Controllers
{
	[Authorize(Roles = StaticDetails.Role_Admin)]
	public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenites = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenites);
        }

        //Create View 
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(Villa=> new SelectListItem
                {
                    Text = Villa.Name,
                    Value = Villa.Id.ToString(),
                })
            };
            return View(amenityVM);
        }

        //Add Amenity to database
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been added successfully";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The villa cannot be added";
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(Villa => new SelectListItem
            {
                Text = Villa.Name,
                Value = Villa.Id.ToString(),

            });
            return View(obj);
        }

        //Update view
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(Villa => new SelectListItem
                {
                    Text= Villa.Name,
                    Value = Villa.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(amenity=>amenity.Id == amenityId)
            };
            if(amenityVM== null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        //update data
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["Success"] = "The Amenity has been updated successfully";
                return RedirectToAction("Index");
            }
            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(Villa => new SelectListItem 
            { 
                Text = Villa.Name,
                Value = Villa.Id.ToString(),
            });
            return View(amenityVM);
        }

        //delete view
        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVM);
        }
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
           Amenity? objFromDb =  _unitOfWork.Amenity.Get(amenity => amenity.Id == amenityVM.Amenity.Id);
           if (objFromDb != null) 
           {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity Has been deleted";
                return RedirectToAction("Index");
           }
            TempData["error"] = "Tha Amenity can  not delete";
            return View();
        }
    }
}
