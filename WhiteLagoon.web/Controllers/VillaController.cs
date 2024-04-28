using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment; // this is the dependency injection for use wwwroot folder
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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

                //image uploading
                if (obj.Image  != null)
                {
                    //chage the name using guid and using GetExtension keep the imge type as it is (png/jpg etc). 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    //set the path in to wwwroot folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }


                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been Added successfully";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "The villa cannot be added";
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


                if (obj.Image != null)
                {
                    //chage the name using guid and using GetExtension keep the imge type as it is (png/jpg etc). 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    //set the path in to wwwroot folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }

                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
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
            Villa? objFromDB = _unitOfWork.Villa.Get(villa => villa.Id == obj.Id);
            if (objFromDB is not null)
            {
                if (!string.IsNullOrEmpty(objFromDB.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDB.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }


                _unitOfWork.Villa.Remove(objFromDB);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index", "Villa");
            }
            TempData["error"] = "THe villa cannot be deleted";
            return View();
        }
    }
}
