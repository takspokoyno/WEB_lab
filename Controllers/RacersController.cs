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
    public class RacersController : Controller
    {
        private readonly RacingContext _context;

        public RacersController(RacingContext context)
        {
            _context = context;
        }

        // GET: Racers
        public async Task<IActionResult> Index()
        {
            var racingContext = _context.Racers.Include(r => r.Team);
            return View(await racingContext.ToListAsync());
        }

        // GET: Racers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Racers == null)
            {
                return NotFound();
            }

            var racer = await _context.Racers
                .Include(r => r.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racer == null)
            {
                return NotFound();
            }

            return View(racer);
        }

        // GET: Racers/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewData["TeamName"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Racers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Sex,BirthDate,TeamId")] Racer racer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(racer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Tournaments");
            }
            ViewData["TeamName"] = new SelectList(_context.Teams, "Id", "Name", racer.TeamId);
            return View(racer);
        }

        // GET: Racers/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Racers == null)
            {
                return NotFound();
            }

            var racer = await _context.Racers.FindAsync(id);
            if (racer == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", racer.TeamId);
            return View(racer);
        }

        // POST: Racers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Sex,BirthDate,TeamId")] Racer racer)
        {
            if (id != racer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(racer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RacerExists(racer.Id))
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
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", racer.TeamId);
            return View(racer);
        }

        // GET: Racers/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Racers == null)
            {
                return NotFound();
            }

            var racer = await _context.Racers
                .Include(r => r.Team)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (racer == null)
            {
                return NotFound();
            }

            return View(racer);
        }

        // POST: Racers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Racers == null)
            {
                return Problem("Entity set 'RacingContext.Racers'  is null.");
            }
            var racer = await _context.Racers.FindAsync(id);
            if (racer != null)
            {
                _context.Racers.Remove(racer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RacerExists(int id)
        {
          return (_context.Racers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
