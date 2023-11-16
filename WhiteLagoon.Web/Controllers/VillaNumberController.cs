using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
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

        [HttpPost]
        public IActionResult Create(VillaNumber villaNumber) {

            if(ModelState.IsValid)
            {
                _db.Add(villaNumber);
                _db.SaveChanges();
                TempData["success"] = "新增成功！";
                return RedirectToAction("Index");
            }
            
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Update(int villa_Number)
        {
            //villaNumber? villaNumber = _db.VillaNumbers.FirstOrDefault(u=>u.Villa_Number == villa_Number);
            //if(villaNumber == null) {
            //    //return NotFound();
            //    return RedirectToAction("Error", "Home");
            //}

            //return View(villaNumber);
            return View();
        }

        [HttpPost]
        public IActionResult Update(VillaNumber villaNumber)
        {

            if (ModelState.IsValid)
            {
                _db.Update(villaNumber);
                _db.SaveChanges();
                TempData["success"] = "更新成功！";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumber villaNumber)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villa.Id);
            if (obj == null)
            {
                TempData["error"] = "刪除失敗！";
                return RedirectToAction("Error", "Home");               
            }

            _db.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "刪除成功！";
            return RedirectToAction("Index");
        }



    }
}
