using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Labka1.Models;

namespace Labka1.Controllers
{
    [Route("api/tournaments")]
    [ApiController]
    public class TournamentsAPIController : ControllerBase
    {
        private RacingContext db = new RacingContext();

        [Produces("application/json")]
        [HttpGet("searchtournaments")]
        [Route("api/tournaments/searchtournaments")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchTournaments()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var tour = db.Tournaments.Where(p => p.Name.Contains(term))
                        .Select(p => p.Name).ToListAsync();
                return Ok(await tour);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}