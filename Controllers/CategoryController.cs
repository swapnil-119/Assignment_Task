using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NimapTask.Data;
using NimapTask.Models;

namespace NimapTask.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                // Simple validation
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    ViewBag.Error = "Category name is required";
                    return View(category);
                }

                // Try to save
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // If successful, redirect
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Show any database errors
                ViewBag.Error = "Error: " + ex.Message;
                return View(category);
            }
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        // POST: Category/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            try
            {
                // Simple validation
                if (string.IsNullOrWhiteSpace(category.CategoryName))
                {
                    ViewBag.Error = "Category name is required";
                    return View(category);
                }

                // Update the category
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                // Clear any caching and redirect
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error updating category: " + ex.Message;
                return View(category);
            }
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}