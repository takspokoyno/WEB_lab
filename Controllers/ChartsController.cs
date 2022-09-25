using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labka1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly Labka1.Models.RacingContext _context;

        public ChartController(Labka1.Models.RacingContext context) {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData() // tournamets
        {
            var tournaments = _context.Tournaments.Include(m=>m.Participations).ToList();
            List<object> tourRacer = new List<object>();
            tourRacer.Add(new[] {"Турнір","Кількість учасників"});
            foreach (var t in tournaments)
            {
                tourRacer.Add(new object[] {t.Name, t.Participations.Count()});
            }
            return new JsonResult(tourRacer);
        }
    }
}
