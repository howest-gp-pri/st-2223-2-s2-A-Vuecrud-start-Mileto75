using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pri.Ca.Core.Entities;
using Pri.Ca.Core.Interfaces.Repositories;
using Pri.Ca.Infrastructure.Data;
using System.Diagnostics;
using Wba.MyfirstWebapp.Web.Models;
using Wba.MyfirstWebapp.Web.ViewModels;

namespace Wba.MyfirstWebapp.Web.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            var user = User.Claims;
            return View();
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}