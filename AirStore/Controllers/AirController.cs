using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirStore.Data;
using AirStore.Models;
using Microsoft.AspNetCore.Authorization;

namespace AirStore.Controllers
{
    public class AirController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AirController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            TempData["Success"] = TempData.ContainsKey("Success") ? TempData["Success"] : false;
            ViewData["IsAdmin"] = User.IsInRole("Admin");
            return View(await _context.Air.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airItem = await _context.Air
                .SingleOrDefaultAsync(m => m.Id == id);
            if (airItem == null)
            {
                return NotFound();
            }

            return View(airItem);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Price")] Air airItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airItem);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airItem = await _context.Air.SingleOrDefaultAsync(m => m.Id == id);
            if (airItem == null)
            {
                return NotFound();
            }
            return View(airItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Price")] Air airItem)
        {
            if (id != airItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirItemExists(airItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(airItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airItem = await _context.Air.SingleOrDefaultAsync(m => m.Id == id);
            _context.Air.Remove(airItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirItemExists(int id)
        {
            return _context.Air.Any(e => e.Id == id);
        }
    }
}
