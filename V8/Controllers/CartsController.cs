using System;
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
    public class CartsController : Controller
    {
        private readonly V8Context _context;
        private readonly UserManager<V8User> _userManager;
        private readonly IHttpContextAccessor _session;

        public CartsController(V8Context context, UserManager<V8User> userManager, IHttpContextAccessor session)
        {
            _userManager = userManager;
            _context = context;
            _session = session;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var cartId = _session.HttpContext.Session.GetString("cartId");

            var carts = _context.Cart
                .Where(c => c.CartId == cartId);

            return View(await carts.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();


            // add to cart
            var checkCount = _session.HttpContext.Session.GetInt32("cartCount");
            int cartCount = checkCount == null ? 0 : (int)checkCount;
            _session.HttpContext.Session.SetInt32("cartCount", --cartCount);

            return RedirectToAction(nameof(Index));
        }

        // GET: Items/Purchase
        [Authorize]
        public async Task<IActionResult> Purchase()
        {
            // get the cart id
            var cartId = _session.HttpContext.Session.GetString("cartId");

            // get the cart items
            var carts = _context.Cart
                .Where(c => c.CartId == cartId);

            // get the buyer
            var buyer = _userManager.GetUserName(User);

            // create the sales
            foreach (Cart cart in carts.ToList())
            {
                // find the item
                var product = await _context.Product
                    .FirstOrDefaultAsync(m => m.Id == cart.Item);

                // update the quantity
                product.Quantity -= cart.Quantity;
                _context.Update(product);

                Sales sale = new Sales { Buyer = buyer, Item = cart.Item, Quantity = cart.Quantity, Price = cart.Price };
                _context.Update(sale);
            }

            // Save the changes
            await _context.SaveChangesAsync();

            // delete cart
            _session.HttpContext.Session.SetString("cartId","");
            _session.HttpContext.Session.SetInt32("cartCount", 0);

            return RedirectToAction(nameof(Index),"Sales");
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}
