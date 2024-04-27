using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.web.ViewModels;

namespace WhiteLagoon.web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            //Inc lude(u => u.Villa) meka dammama auto ma model eke vills table ekath ekka
            //forign key hadala thiyna nisa Villa table ekath ekka join ekak gahagena Villa wala onnoma delails denwa
            //(navigation proprty kiyla kiynne) thwath ekak danna onenan then kiyula ayeth ekak dnna puluwan 
            return View(villaNumbers);
        }

       // public IActionResult Create()
       // {
            // u=> meken kiyawenne it's like saying "for each villa (u) in the list, do the following:"
            // ekiynne collection eke thiyna okkoma element text ekai value ekai illana widita map kranna kiyla
          //  IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(hi=> new SelectListItem
          //  {
           //     Text= hi.Name,
           //     Value=hi.Id.ToString(),
           // });
            //me view data function eken thmai uda gaththa list eka view ekata ywnne. return eken ywanna ba. 
            // ewagema thawa ViewBag kiyla ekak thiynwa ekenuth krnne temparaly data controller eken view ekata data aran yna eka.
            // ViewBag.list = list;
            // ViewData kiynne type of data dictionary ekak.eth view bag dynamic type property ekak
          //  ViewData["list"] = list;
           //  return View();
       // }


        // uda karala thiyna eka hri unth eka best practice eka newei. 
       // dropdown eka disply krana vidiha
        public IActionResult Create()
        {
            // aluthin model ekak hdalanwa VM kiyla. eke select ekata enna one tika dala pahala widihata
            // e model ekata data pass kranwa. itpasse view ekata e VM eken data gannwa
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                })
            };
            return View(villaNumberVM);
        }


        //Add villa number
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumber = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

            if(ModelState.IsValid && !roomNumber)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.VillaNumber.Save();
                TempData["success"] = "The villa Number has been Added successfully";
                return RedirectToAction("Index");
            }
            if (roomNumber)
            {
                TempData["error"] = "The room number is already exists";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            return View(obj);
        }

        //view the Update View
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if(villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        //update the viewed 
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.VillaNumber.Save();
                TempData["success"] = "The villa Number has been Updated Succsessfully";
                return RedirectToAction("Index");
            }
            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString(),
            });
            return View(villaNumberVM);
        }

        //disply delete view
        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        //delete
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if(objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.VillaNumber.Save();
                TempData["success"] = "The villa Number has beed deleted";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The vills could not be delete";
            return View();
        }
    }
}
