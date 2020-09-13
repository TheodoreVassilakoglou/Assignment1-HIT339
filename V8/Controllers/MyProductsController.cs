using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using V8.Data;
using V8.Models;

namespace V8.Controllers
{
    public class MyProductsController : Controller
    {
        private readonly V8Context _context;
        private readonly UserManager<V8User> _userManager;


        public MyProductsController(V8Context context, UserManager<V8User> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: MyProducts
        public async Task<IActionResult> Index()
        {
            var v8Context = _context.Product.Include(p => p.User);


            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);
            var MyItemsForSale = _context.Product.Where(u => u.UserId == _userManager.GetUserId(HttpContext.User));
            return View(await MyItemsForSale.ToListAsync());
        }

        // GET: MyProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: MyProducts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<V8User>(), "Id", "Id");
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);
            return View();
        }

        // POST: MyProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserId,Name,Price,Photo")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<V8User>(), "Id", "Id", product.UserId);
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);
            return View(product);
        }

        // GET: MyProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<V8User>(), "Id", "Id", product.UserId);
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);
            return View(product);
        }

        // POST: MyProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserId,Name,Price,Photo")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["UserId"] = new SelectList(_context.Set<V8User>(), "Id", "Id", product.UserId);
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);
            return View(product);
        }

        // GET: MyProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: MyProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
