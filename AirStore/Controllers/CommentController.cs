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
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentController(
             UserManager<ApplicationUser> userManager,
             ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            TempData["Success"] = TempData.ContainsKey("Success") ? TempData["Success"] : false;

            List<Comment> comments = _context.Comment.Include(m => m.User).OrderByDescending(m => m.CreatedAt).Take(5).ToList();

            return View(comments);
        }

        public async Task<IActionResult> ViewAll(int? page)
        {
            int PageSize = 10;

            IQueryable<Comment> comments = _context.Comment.Include(m => m.User).AsNoTracking();

            return View(await PaginatedList<Comment>.CreateAsync(comments, page ?? 1, PageSize));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text")] Comment comment)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            comment.UserId = user.Id;
            if (comment.Text.Length > 0)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                TempData["Success"] = true;
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }
    }
}