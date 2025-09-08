using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NimapTask.Data;
using NimapTask.Models;
using NimapTask.ViewModels;

namespace NimapTask.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 10;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(int page = 1)
        {
            var totalRecords = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / PageSize);

            // Server-side pagination
            var products = await _context.Products
                .Include(p => p.Category)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(p => new ProductViewModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            var viewModel = new ProductListViewModel
            {
                Products = products,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                PageSize = PageSize
            };

            return View(viewModel);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        
        }

        // POST: Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                // Simple validation
                if (string.IsNullOrWhiteSpace(product.ProductName))
                {
                    ViewBag.Error = "Product name is required";
                    ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                if (product.CategoryId <= 0)
                {
                    ViewBag.Error = "Please select a category";
                    ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                // Try to save
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                // If successful, redirect
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Show any database errors
                ViewBag.Error = "Error: " + ex.Message;
                ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            try
            {
                // Simple validation
                if (string.IsNullOrWhiteSpace(product.ProductName))
                {
                    ViewBag.Error = "Product name is required";
                    ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                if (product.CategoryId <= 0)
                {
                    ViewBag.Error = "Please select a category";
                    ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                    return View(product);
                }

                // Update the product
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                // Redirect to list
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error updating product: " + ex.Message;
                ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", product.CategoryId);
                return View(product);
            }
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}