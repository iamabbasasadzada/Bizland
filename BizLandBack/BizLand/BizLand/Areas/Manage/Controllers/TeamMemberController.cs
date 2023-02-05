using BizLand.DAL;
using BizLand.Models;
using BizLand.Utilies.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BizLand.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class TeamMemberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamMemberController(AppDbContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Authorize]
        public IActionResult Index(int page=1)
        {
           
            return View(_context.TeamMembers.ToPagedList(page,3));
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(TeamMember teamMember)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (_context.TeamMembers.FirstOrDefault(t=>t.FullName == teamMember.FullName) != null)
            {
                ModelState.AddModelError("FullName", "This FullName already in DB");
                return View();
            }
            if(teamMember.Photo == null)
            {
                ModelState.AddModelError("Photo", "You must choose file");
                return View();
            }
            if (!teamMember.Photo.CheckSize(50000))
            {
                ModelState.AddModelError("Photo", "File must be lower than 50000 kb");
                return View();
            }
            if (!teamMember.Photo.CheckType("image/"))
            {
                ModelState.AddModelError("Photo", "File must be image");
                return View();
            }
            string savePath = Path.Combine(_env.WebRootPath, "assets", "images");
            teamMember.Image = await teamMember.Photo.SaveFileAsync(savePath);
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            TeamMember team = _context.TeamMembers.FirstOrDefault(t => t.Id == id);
            if (team == null) return BadRequest();
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(TeamMember teamMember)
        {
            TeamMember teamToUpdate = _context.TeamMembers.FirstOrDefault(t => t.Id == teamMember.Id);
            if (teamToUpdate == null) return BadRequest();
            if (teamToUpdate.FullName != teamMember.FullName)
            {
                if (_context.TeamMembers.FirstOrDefault(t => t.FullName == teamMember.FullName) != null)
                {
                    ModelState.AddModelError("FullName", "This FullName already in DB");
                    return View();
                }
            }
            if (teamMember.Photo != null)
            {
                if (!teamMember.Photo.CheckSize(50000))
                {
                    ModelState.AddModelError("Photo", "File must be lower than 50000 kb");
                    return View();
                }
                if (!teamMember.Photo.CheckType("image/"))
                {
                    ModelState.AddModelError("Photo", "File must be image");
                    return View();
                }
                string savePath = Path.Combine(_env.WebRootPath, "assets", "images");
                teamMember.Image = await teamMember.Photo.SaveFileAsync(savePath);
                if(System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "images", teamToUpdate.Image)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "images", teamToUpdate.Image));
                }
                teamToUpdate.Image = teamMember.Image;
            }
            teamToUpdate.FullName = teamMember.FullName;
            teamToUpdate.Position = teamMember.Position;
            teamToUpdate.InstaLink = teamMember.InstaLink;
            teamToUpdate.FacebookLink = teamMember.FacebookLink;
            teamToUpdate.TwitterLink = teamMember.TwitterLink;
            teamToUpdate.LinkedinLink = teamMember.LinkedinLink;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int id)
        {
            TeamMember team = _context.TeamMembers.FirstOrDefault(t => t.Id == id);
            if (team == null) return BadRequest();
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "images", team.Image)))
            {
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "images", team.Image));
            }
            _context.TeamMembers.Remove(team);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
