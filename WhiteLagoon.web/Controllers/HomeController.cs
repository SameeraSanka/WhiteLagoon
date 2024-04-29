using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.web.Models;
using WhiteLagoon.web.ViewModels;

namespace WhiteLagoon.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                // methana includeProperties walata dnne Villa model eke Amenity walin gaththa eka
                VillaList = _unitOfWork.Villa.GetAll(includeProperties : "VillaAmenity"),
                Nights = 1,
                CheckInData = DateOnly.FromDateTime(DateTime.Now),
            };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
        public IActionResult Error()
        {
            return View();
        }
    }
}
