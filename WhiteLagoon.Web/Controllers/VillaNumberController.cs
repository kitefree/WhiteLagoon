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
            //以view model 回傳
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })

            };

            
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM villaNumberVm) {
            
            //移除不必要驗證屬性
            ModelState.Remove("VillaNumber.Villa");
                      
            //第一關驗證
            if (!ModelState.IsValid)
            {
                villaNumberVm.VillaList =
                 _db.Villas.ToList().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });

                TempData["error"] = "新增失敗！";
                return View(villaNumberVm);
            }

            //第二關驗證
            bool isNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);

            if (isNumberExists)
            {
                villaNumberVm.VillaList =
                 _db.Villas.ToList().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });
                TempData["error"] = $"新增失敗！Villa_Number {villaNumberVm.VillaNumber.Villa_Number.ToString()}已存在！";
                return View(villaNumberVm);
            }

            //闖關成功，寫入資料庫
            _db.Add(villaNumberVm.VillaNumber);
            _db.SaveChanges();
            TempData["success"] = "新增成功！";
            return RedirectToAction(nameof(Index));

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
        public IActionResult Update(VillaNumberVM villaNumberVm)
        {
            //移除不必要驗證屬性
            ModelState.Remove("VillaNumber.Villa");

            //第一關驗證
            if (!ModelState.IsValid)
            {
                villaNumberVm.VillaList =
                 _db.Villas.ToList().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });

                TempData["error"] = "更新失敗！";
                return View(villaNumberVm);
            }

            //闖關成功，寫入資料庫
            _db.Update(villaNumberVm.VillaNumber);
            _db.SaveChanges();
            TempData["success"] = "更新成功！";
            return RedirectToAction(nameof(Index));

            

        }

        public IActionResult Delete(int villaNumberId)
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
        public IActionResult Delete(VillaNumberVM villaNumberVm)
        {
            VillaNumber? obj = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberVm.VillaNumber.Villa_Number);
            if (obj == null)
            {
                TempData["error"] = "刪除失敗！";
                return RedirectToAction("Error", "Home");               
            }

            _db.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "刪除成功！";
            return RedirectToAction(nameof(Index));
        }



    }
}
