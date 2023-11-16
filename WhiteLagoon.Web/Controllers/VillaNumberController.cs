using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

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
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })

            };

            
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumber villaNumber) {

            ModelState.Remove("Villa");
            if(ModelState.IsValid)
            {
                _db.Add(villaNumber);
                _db.SaveChanges();
                TempData["success"] = "新增成功！";
                return RedirectToAction("Index");
            }
            
            return View();
        }


        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            
            if (villaNumberVM.VillaNumber == null)
            {
                //return NotFound();
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
            
        }

        [HttpPost]
        public IActionResult Update(VillaNumber villaNumber)
        {
            VillaNumber? obj = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumber.Villa_Number);
            if (obj is not null)
            {
                TempData["error"] = $"Villa_Number 已存在";                
            }
            else if (ModelState.IsValid)
            {
                _db.Update(obj);
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
            VillaNumber? obj = _db.VillaNumbers.FirstOrDefault(u => u.VillaId == villaNumber.VillaId);
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
