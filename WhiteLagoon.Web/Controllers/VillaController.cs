using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa) {
            if(villa.Name == villa.Description)
            {
                ModelState.AddModelError("", "名稱與描述不能一致");
            }
            if(ModelState.IsValid)
            {
                _db.Add(villa);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View();
        }
        

        public IActionResult Edit()
        {
            return View();
        }
    }
}
