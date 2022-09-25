using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labka1.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace Labka1.Controllers
{
    public class TeamsController : Controller
    {
        private readonly RacingContext _context;

        public TeamsController(RacingContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
              return _context.Teams != null ? 
                          View(await _context.Teams.ToListAsync()) :
                          Problem("Entity set 'RacingContext.Teams'  is null.");
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Sponsors", new { teamId = team.Id, name = team.Name});
            //return View(team);
        }

        // GET: Teams/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
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
            return View(team);
        }

        // GET: Teams/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'RacingContext.Teams'  is null.");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку команд)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва команди. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Team newTeam;
                                var c = (from team in _context.Teams
                                         where team.Name.Contains(worksheet.Name)
                                         select team).ToList();
                                if (c.Count > 0)
                                {
                                    newTeam = c[0];
                                }
                                else
                                {
                                    newTeam = new Team();
                                    newTeam.Name = worksheet.Name;
                                    //додати в контекст
                                    _context.Teams.Add(newTeam);
                                }
                                //перегляд усіх рядків (гонщиків)
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Racer racer = new Racer();
                                        racer.Name = row.Cell(1).Value.ToString();
                                        racer.Sex = row.Cell(2).Value.ToString();
                                        racer.BirthDate = DateTime.Parse(row.Cell(3).Value.ToString());
                                        racer.Team = newTeam;
                                        _context.Racers.Add(racer);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 4; i <= 7; i++) // декілька участей
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Tournament tournament;
                                                var a = (from tour in _context.Tournaments
                                                         where tour.Name.Contains(row.Cell(i).Value.ToString())
                                                         select tour).ToList();
                                                if (a.Count > 0)
                                                {
                                                    tournament = a[0];
                                                }
                                                else
                                                {
                                                    tournament = new Tournament();
                                                    tournament.Name = row.Cell(i).Value.ToString();
                                                    tournament.Reward = 100000;
                                                    //додати в контекст
                                                    _context.Add(tournament);
                                                }
                                                Participation prt = new Participation();
                                                prt.Racer = racer;
                                                prt.Tournament = tournament;
                                                _context.Participations.Add(prt);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //Геть з України, виняток некрасівий
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var teams = _context.Teams.Include("Racers").ToList();
                foreach (var c in teams)
                {
                    var worksheet = workbook.Worksheets.Add(c.Name.Substring(0,25));
                    worksheet.Cell("A1").Value = "Ім'я";
                    worksheet.Cell("B1").Value = "Стать";
                    worksheet.Cell("C1").Value = "Дата народження";
                    worksheet.Cell("D1").Value = "Участь 1";
                    worksheet.Cell("E1").Value = "Участь 2";
                    worksheet.Cell("F1").Value = "Участь 3";
                    worksheet.Cell("G1").Value = "Участь 4";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var racers = c.Racers.ToList();
                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < racers.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = racers[i].Name;
                        worksheet.Cell(i + 2, 2).Value = racers[i].Sex;
                        worksheet.Cell(i + 2, 3).Value = racers[i].BirthDate;
                        var ab = _context.Participations.Where(a => a.RacerId == racers[i].Id).Include("Tournament").ToList();
                        //більше 4-ох нікуди писати
                        int j = 0;
                        foreach (var a in ab)
                        {
                            if (j < 4)
                            {
                                worksheet.Cell(i + 2, j + 4).Value = a.Tournament.Name;
                                j++;
                            }
                        }
                    }
                }
                using (var stream = new MemoryStream())

                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                           {FileDownloadName = $"racing.xlsx"};
                }
            }
        }
    }
}
