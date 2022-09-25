using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labka1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Labka1.Controllers
{
    public class PartsController : Controller
    {
        private readonly RacingContext _context;
        
        public PartsController(RacingContext context)
        {
            _context = context;
        }

        // GET: Parts
        public async Task<IActionResult> Index(int? carId, string model, string brand)
        {
            if (carId == null) return RedirectToAction("Cars", "Index");

            //ViewBag.CarId = id;
            ViewBag.CarBrand = brand;
            ViewBag.CarModel = model;
            var partsByCar = _context.Parts.Where(p => p.CarId == carId); 
            ViewData["currentCarId"]=carId;
            List<Part> result = await partsByCar.ToListAsync();
            return View(result);
        }
        
        // GET: Parts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .Include(p => p.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }

            return View(part);
        }

        // GET: Parts/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int carId)
        {
            ViewData["currentCar"] = _context.Cars.FirstOrDefault(c => c.Id == carId);
            ViewBag.CarBrand = _context.Cars.Where(c => c.Id == carId).FirstOrDefault().Brand;
            ViewBag.CarModel = _context.Cars.Where(c => c.Id == carId).FirstOrDefault().Model;
            return View();
        }

        // POST: Parts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CarId")] Part part)
        {
            if (ModelState.IsValid)
            {
                _context.Add(part);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Parts", routeValues: new {carId=part.CarId, _context.Cars.Where(c => c.Id == part.CarId).FirstOrDefault().Brand, _context.Cars.Where(c => c.Id == part.CarId).FirstOrDefault().Model });
            }
            ViewData["currentCar"] = _context.Cars.FirstOrDefault(c => c.Id == part.CarId);
            return View(part);
        }

        // GET: Parts/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts.FindAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            //ViewBag.CarBrand = _context.Cars.Where(c => c.Id == id).FirstOrDefault().Brand;
            //ViewBag.CarModel = _context.Cars.Where(c => c.Id == id).FirstOrDefault().Model;
            //ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Brand", part.CarId);
            ViewData["currentCar"] = _context.Cars.FirstOrDefault(c => c.Id == part.CarId);
            return View(part);
        }

        // POST: Parts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CarId")] Part part)
        {
            if (id != part.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(part);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartExists(part.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Parts", routeValues: new { carId = part.CarId, _context.Cars.Where(c => c.Id == part.CarId).FirstOrDefault().Brand, _context.Cars.Where(c => c.Id == part.CarId).FirstOrDefault().Model });
            }
            ViewData["currentCar"] = _context.Cars.FirstOrDefault(c => c.Id == part.CarId);
            return View(part);
        }

        // GET: Parts/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Parts == null)
            {
                return NotFound();
            }

            var part = await _context.Parts
                .Include(p => p.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (part == null)
            {
                return NotFound();
            }
            ViewData["currentCar"] = _context.Cars.FirstOrDefault(c => c.Id == part.CarId);
            return View(part);
        }

        // POST: Parts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Parts == null)
            {
                return Problem("Entity set 'RacingContext.Parts'  is null.");
            }
            var part = await _context.Parts.FindAsync(id);
            if (part != null)
            {
                _context.Parts.Remove(part);
            }
            
            await _context.SaveChangesAsync();
            ViewData["currentCar"] = _context.Cars.FirstOrDefault(c => c.Id == part.CarId);
            return RedirectToAction(nameof(Index), "Parts", routeValues: new { carId = part.CarId, _context.Cars.Where(c => c.Id == part.CarId).FirstOrDefault().Brand, _context.Cars.Where(c => c.Id == part.CarId).FirstOrDefault().Model });
        }

        private bool PartExists(int id)
        {
          return (_context.Parts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}