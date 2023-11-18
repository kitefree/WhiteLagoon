using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                ModelState.AddModelError("name", "名稱與描述不能一致");
            }
            if(ModelState.IsValid)
            {
                if(villa.Image !=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        villa.Image.CopyTo(fileStream);
                        villa.ImageUrl = @"\images\VillaImages\" + fileName;
                    }

                }
                else
                {
                    villa.ImageUrl = "https://placehold.co/600x400";
                }


                _unitOfWork.Villa.Add(villa);
                _unitOfWork.Save();
                
                TempData["success"] = "新增成功！";
                return RedirectToAction(nameof(Index));
            }
            
            return View();
        }

        public IActionResult Update(int villaId)
        {            
            Villa? villa = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (villa == null) {
                
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (villa.Name == villa.Description)
            {
                ModelState.AddModelError("name", "名稱與描述不能一致");
            }
            if (ModelState.IsValid)
            {

                if (villa.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    if(!string.IsNullOrEmpty(villa.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        villa.Image.CopyTo(fileStream);
                        villa.ImageUrl = @"\images\VillaImages\" + fileName;
                    }

                }

                _unitOfWork.Villa.Update(villa);
                _unitOfWork.Save();
                
                TempData["success"] = "更新成功！";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villa.Id);
            if (obj == null)
            {
                TempData["error"] = "刪除失敗！";
                return RedirectToAction("Error", "Home");               
            }

            if (!string.IsNullOrEmpty(obj.ImageUrl))
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Villa.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "刪除成功！";
            return RedirectToAction(nameof(Index));
        }



    }
}
