using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenitys = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenitys);
        }

        public IActionResult Create()
        {
            //以view model 回傳
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).ToList()

            };

            
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM) 
        {
            //第一關驗證
            if (amenityVM.Amenity.VillaId == 0)
            {
                amenityVM.VillaList =
                _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                TempData["error"] = "新增失敗！請選擇Villa選項";
                return View(amenityVM);
            }
            

            

            //第二關驗證
            if (!ModelState.IsValid)
            {
                amenityVM.VillaList =
                 _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });

                TempData["error"] = "新增失敗！";
                return View(amenityVM);
            }


            //闖關成功，寫入資料庫
            _unitOfWork.Amenity.Add(amenityVM.Amenity);
            _unitOfWork.Save();
            TempData["success"] = "新增成功！";
            return RedirectToAction(nameof(Index));

        }


        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).ToList(),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };
            
            if (amenityVM.Amenity == null)
            {
                //return NotFound();
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
            
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            //移除不必要驗證屬性
            ModelState.Remove("Ament.Villa");

            //第一關驗證
            if (!ModelState.IsValid)
            {
                amenityVM.VillaList =
                 _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 }).ToList();

                TempData["error"] = "更新失敗！";
                return View(amenityVM);
            }

            //闖關成功，寫入資料庫
            _unitOfWork.Amenity.Update(amenityVM.Amenity);
            _unitOfWork.Save();
            TempData["success"] = "更新成功！";
            return RedirectToAction(nameof(Index));

            

        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }).ToList(),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)
            };

            if (amenityVM.Amenity == null)
            {
                //return NotFound();
                return RedirectToAction("Error", "Home");
            }


            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? obj = _unitOfWork.Amenity.Get(u => u.Id == amenityVM.Amenity.Id);
            if (obj == null)
            {
                TempData["error"] = "刪除失敗！";
                return RedirectToAction("Error", "Home");               
            }

            _unitOfWork.Amenity.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "刪除成功！";
            return RedirectToAction(nameof(Index));
        }



    }
}
