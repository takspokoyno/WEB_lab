using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Labka1.Models;

namespace Labka1.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamsAPIController : ControllerBase
    {
        private RacingContext db = new RacingContext();

        [Produces("application/json")]
        [HttpGet("searchteams")]
        [Route("api/teams/searchteams")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchTeams()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var teams = db.Teams.Where(p => p.Name.Contains(term))
                        .Select(p => p.Name).ToListAsync();
                return Ok(await teams);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}