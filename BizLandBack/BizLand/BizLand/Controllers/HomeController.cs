using BizLand.DAL;
using BizLand.Models;
using BizLand.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BizLand.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                teamMembers = _context.TeamMembers.OrderByDescending(f => f.Id).Take(4).ToList(),
                features = _context.Features.OrderByDescending(f => f.Id).Take(6).ToList()
            };
            
            return View(homeVM);
        }
    }
}
