using EmployeeMVC.Models;
using EmployeeMVC.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeeMVC.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IRepositoryWrapper repositoryWrapper, IConfiguration configuration) : base(repositoryWrapper, configuration)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<JsonResult> GetHolidays(DateTime start, DateTime end)
        {
            var Holidaylist = await _repositoryWrapper.Holiday.LoadHolidaysWithInterval(start, end);
            var Leavelist = await _repositoryWrapper.Leave.LoadLeaveList(start, end);

            var events = new List<dynamic>();
            foreach (var item in Holidaylist)
            {
                events.Add(new
                {
                    id = item.Id,
                    title = item.Name,
                    start = item.Date.ToString("yyyy-MM-dd"),
                    color = "red",
                    url = "/Holidays/Details/" + item.Id
                });
            }

            foreach (var item in Leavelist)
            {
                events.Add(new
                {
                    id = item.Id,
                    title = item.EmployeeId + " " + item.EmployeeName,
                    start = item.Date.ToString("yyyy-MM-dd"),
                    color = "green",
                    url = "/Leaves/Details/" + item.Id
                });
            }

            return Json(events);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}