﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using V8.Data;
using V8.Models;

namespace V8.Controllers
{
    public class ProductsController : Controller
    {
        private readonly V8Context _context;
        private readonly UserManager<V8User> _userManager;
        private readonly IHttpContextAccessor _session;


        public ProductsController(V8Context context, UserManager<V8User> userManager, IHttpContextAccessor session)
        {
            _context = context;
            _userManager = userManager;
            _session = session;

        }

        // GET: Products
        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from m in _context.Product
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

       
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);


            var context = products.Include(p => p.User);

            return View(await context.ToListAsync());
            //theres suppose to be a products.tolistasync()); and it works 

            //nvm its pre 
            
           
        }

        /*
        public async Task<IActionResult> index(string searchString)
        {
            var products = from m in _context.Product
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString));
            }

            return View(await products.ToListAsync());
        } */


        [Authorize]
        // GET: Products/Details/5
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

        // GET: Products/Create
        [Authorize]

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<V8User>(), "Id", "Id");

            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);
            ViewBag.UserName = _userManager.GetUserName(HttpContext.User);

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,UserId,Name,Price,Photo,Quantity")] Product product)
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

        // GET: Products/Edit/5
        [Authorize]

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



        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,UserId,Name,Price,Photo,Quantity")] Product product)
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

        // GET: Products/Delete/5
        [Authorize]

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

        // POST: Products/Delete/5
        [Authorize]
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

        // GET: Items/Purchase/5
        [Authorize]

        public async Task<IActionResult> Purchase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Items/Purchase/5
        [Authorize]
        [HttpPost, ActionName("Purchase")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseConfirmed([Bind("Item,Quantity,Price")] Cart cart)
        {
            // get or create a cart id
            string cartId = _session.HttpContext.Session.GetString("cartId");

            if (string.IsNullOrEmpty(cartId) == true) cartId = Guid.NewGuid().ToString();

            // use the cart id            
            cart.CartId = cartId.ToString();
            cart.Price = cart.Price * cart.Quantity;
            // make the sale
            _context.Add(cart);

            // Save the changes
            await _context.SaveChangesAsync();

            // add to cart
            var checkCount = _session.HttpContext.Session.GetInt32("cartCount");
            int cartCount = checkCount == null ? 0 : (int)checkCount;
            _session.HttpContext.Session.SetString("cartId", cartId.ToString());
            _session.HttpContext.Session.SetInt32("cartCount", ++cartCount);

            return Redirect("~/Carts");

        }

        private bool ItemsExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }






    }
}
