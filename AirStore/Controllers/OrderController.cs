using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirStore.Data;
using AirStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirStore.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Buy(int? id)
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

        [HttpPost, ActionName("Buy")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyConfirmed(int id)
        {
            var airItem = await _context.Air
                   .SingleOrDefaultAsync(m => m.Id == id);

            if (airItem == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);
            Order order = new Order { AirId = airItem.Id, UserId = user.Id };
            _context.Add(order);
            _context.SaveChanges();

            TempData["Success"] = true;
            return RedirectToAction("Index", "Air");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? page)
        {
            TempData["Shipped"] = TempData.ContainsKey("Shipped") ? TempData["Shipped"] : false;

            int PageSize = 10;
            IQueryable<Order> orders = _context.Order.Include(m => m.Air).Include(m => m.User).AsNoTracking();

            return View(await PaginatedList<Order>.CreateAsync(orders, page ?? 1, PageSize));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyOrders()
        {
            TempData["Delivered"] = TempData.ContainsKey("Delivered") ? TempData["Delivered"] : false;
            ApplicationUser user = await _userManager.GetUserAsync(User);
            return View(_context.Order.Include(m => m.Air).Where(m => m.UserId == user.Id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Ship(int id)
        {
            Order order = await _context.Order.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if (!order.Shipped)
            {
                order.Shipped = true;
            }
            _context.Update(order);
            await _context.SaveChangesAsync();
            TempData["Shipped"] = true;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Accept(int id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            Order order = await _context.Order.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            if(order.User.Id != user.Id)
            {
                return NotFound();
            }
            if (order.Shipped)
            {
                order.Delivered = true;
            }
            _context.Update(order);
            await _context.SaveChangesAsync();
            TempData["Delivered"] = true;
            return RedirectToAction(nameof(MyOrders));
        }
    }
}