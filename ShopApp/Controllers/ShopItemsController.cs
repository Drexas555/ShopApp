using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data;
using ShopApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ShopApp.Controllers
{
    public class ShopItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShopItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ShopItems
        public async Task<IActionResult> Index()
        {
              return View(await _context.ShopItem.ToListAsync());
        }

        // GET: ShopItems/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: ShopItems/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.ShopItem.Where( i=> i.Name.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: ShopItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopItem == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // GET: ShopItems/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShopItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] ShopItem shopItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopItem);
        }

        // GET: ShopItems/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopItem == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItem.FindAsync(id);
            if (shopItem == null)
            {
                return NotFound();
            }
            return View(shopItem);
        }

        // POST: ShopItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] ShopItem shopItem)
        {
            if (id != shopItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopItemExists(shopItem.Id))
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
            return View(shopItem);
        }

        // GET: ShopItems/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopItem == null)
            {
                return NotFound();
            }

            var shopItem = await _context.ShopItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopItem == null)
            {
                return NotFound();
            }

            return View(shopItem);
        }

        // POST: ShopItems/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShopItem'  is null.");
            }
            var shopItem = await _context.ShopItem.FindAsync(id);
            if (shopItem != null)
            {
                _context.ShopItem.Remove(shopItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopItemExists(int id)
        {
          return _context.ShopItem.Any(e => e.Id == id);
        }
    }
}
