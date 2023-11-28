using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateTime.Now,
            };

            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateTime checkInDate)
        {

            var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumbersList = _unitOfWork.VillaNumber.GetAll().ToList();
            var bookiedVillas = _unitOfWork.Booking.GetAll(u => u.Status == SD.StatusApproved || u.Status == SD.StatusCheckedIn).ToList();


            foreach (var villa in villaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumbersList, checkInDate, nights, bookiedVillas);
                villa.IsAvailable = roomAvailable > 0 ? true : false;
            }

            HomeVM homeVM = new()
            {
                VillaList = villaList,
                Nights = nights,
                CheckInDate = checkInDate,
            };


            return PartialView("_VillaList", homeVM);
        }

        
        public IActionResult Privacy()
        {
            return View();
        }


        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}