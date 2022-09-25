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
    public class ParticipationsController : Controller
    {
        private readonly RacingContext _context;

        public ParticipationsController(RacingContext context)
        {
            _context = context;
        }

        // GET: Participations
        public async Task<IActionResult> Index(int? tournamentId, string tournamentName)
        {

            ViewBag.TournamentName = tournamentName;
            //ViewBag.TournamentId = tournamentId;
            ViewData["currentTournamentId"] = tournamentId;
            var racingContext = _context.Participations.Include(p => p.Racer).Include(p => p.Tournament).Where(p=>p.TournamentId == tournamentId);
            List<Participation> result = await racingContext.ToListAsync();
            return View(result);
        }

        // GET: Participations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Participations == null)
            {
                return NotFound();
            }

            var participation = await _context.Participations
                .Include(p => p.Racer)
                .Include(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participation == null)
            {
                return NotFound();
            }

            return View(participation);
        }

        // GET: Participations/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create(int tournamentId)
        {
            ViewBag.TournamentName = _context.Tournaments.Where(c => c.Id == tournamentId).FirstOrDefault().Name;
            ViewData["currentTournament"] = _context.Tournaments.FirstOrDefault(c => c.Id == tournamentId);
            ViewData["RacerId"] = new SelectList(_context.Racers, "Id", "Name");
            //ViewData["currentTournamentId"] = _context.Tournaments.Where(c => c.Id == tournamentId).FirstOrDefault().Id;//new SelectList(_context.Tournaments, "Id", "Name");
            return View();
        }

        // POST: Participations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RacerId,TournamentId")] Participation participation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(participation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Participations", routeValues: new {tournamentId=participation.TournamentId, _context.Tournaments.Where(c => c.Id == participation.TournamentId).FirstOrDefault().Name});
            }
            ViewData["currentTournament"] = _context.Tournaments.FirstOrDefault(c => c.Id == participation.TournamentId);
            ViewData["TournamentName"] = new SelectList(_context.Teams, "Id", "Name", participation.TournamentId);
            //ViewData["RacerId"] = new SelectList(_context.Racers, "Id", "Name", participation.RacerId);
            //ViewData["TournamentId"] = _context.Tournaments.Where(c => c.Id == participation.TournamentId).FirstOrDefault().Id;//new SelectList(_context.Tournaments, "Id", "Name", participation.TournamentId);
            return View(participation);
        }

        // GET: Participations/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Participations == null)
            {
                return NotFound();
            }

            var participation = await _context.Participations.FindAsync(id);
            if (participation == null)
            {
                return NotFound();
            }
            ViewData["currentTournament"] = _context.Tournaments.FirstOrDefault(c => c.Id == participation.TournamentId);
            ViewData["RacerId"] = new SelectList(_context.Racers, "Id", "Id", participation.RacerId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Id", participation.TournamentId);
            return View(participation);
        }

        // POST: Participations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RacerId,TournamentId")] Participation participation)
        {
            if (id != participation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipationExists(participation.Id))
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
            ViewData["currentTournament"] = _context.Tournaments.FirstOrDefault(c => c.Id == participation.TournamentId);
            ViewData["RacerId"] = new SelectList(_context.Racers, "Id", "Id", participation.RacerId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Id", participation.TournamentId);
            return View(participation);
        }

        // GET: Participations/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Participations == null)
            {
                return NotFound();
            }

            var participation = await _context.Participations
                .Include(p => p.Racer)
                .Include(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participation == null)
            {
                return NotFound();
            }
            ViewData["currentTournament"] = _context.Tournaments.FirstOrDefault(c => c.Id == participation.TournamentId);
            return View(participation);
        }

        // POST: Participations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Participations == null)
            {
                return Problem("Entity set 'RacingContext.Participations'  is null.");
            }
            var participation = await _context.Participations.FindAsync(id);
            if (participation != null)
            {
                _context.Participations.Remove(participation);
            }
            ViewData["currentTournament"] = _context.Tournaments.FirstOrDefault(c => c.Id == participation.TournamentId);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Participations", routeValues: new {tournamentId=participation.TournamentId, _context.Tournaments.Where(c => c.Id == participation.TournamentId).FirstOrDefault().Name });
        }

        private bool ParticipationExists(int id)
        {
          return (_context.Participations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
