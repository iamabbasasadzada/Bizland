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
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public IActionResult Index(int page = 1)
        {

            return View(_context.Features.ToPagedList(page, 3));
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Features feature)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (_context.Features.FirstOrDefault(t => t.Name == feature.Name) != null)
            {
                ModelState.AddModelError("FullName", "This FullName already in DB");
                return View();
            }
           
            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            Features feature = _context.Features.FirstOrDefault(t => t.Id == id);
            if (feature == null) return BadRequest();
            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Features feature)
        {
            Features featureToUpdate = _context.Features.FirstOrDefault(t => t.Id == feature.Id);
            if (featureToUpdate == null) return BadRequest();
            if (featureToUpdate.Name != feature.Name)
            {
                if (_context.Features.FirstOrDefault(t => t.Name == feature.Name) != null)
                {
                    ModelState.AddModelError("Name", "This FullName already in DB");
                    return View();
                }
            }
            featureToUpdate.Name = feature.Name;
            featureToUpdate.IconClass = feature.IconClass;
            featureToUpdate.Desc = feature.Desc;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int id)
        {
            Features feature = _context.Features.FirstOrDefault(t => t.Id == id);
            if (feature == null) return BadRequest();
           
            _context.Features.Remove(feature);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
