using Microsoft.AspNetCore.Mvc;
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


    }
}
