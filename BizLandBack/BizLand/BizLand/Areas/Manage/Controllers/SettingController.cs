using BizLand.DAL;
using BizLand.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BizLand.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index(int page=1)
        {
            
            return View(_context.Settings.ToPagedList(page,3));
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            Setting setting = _context.Settings.FirstOrDefault(t => t.Id == id);
            if (setting == null) return BadRequest();
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Setting setting)
        {
            Setting settingToUpdate = _context.Settings.FirstOrDefault(t => t.Id == setting.Id);
            if (settingToUpdate == null) return BadRequest();
            if(settingToUpdate.Key != setting.Key)
            {
                if (_context.Settings.FirstOrDefault(t => t.Key == setting.Key) != null)
                {
                    ModelState.AddModelError("Key", "This Key already in DB");
                    return View();
                }
            }
            settingToUpdate.Key = setting.Key;
            settingToUpdate.Value = setting.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
