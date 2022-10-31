using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Labka1.Models;

namespace Labka1.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsAPIController : ControllerBase
    {
        private RacingContext db = new RacingContext();

        [Produces("application/json")]
        [HttpGet("searchbrands")]
        [Route("api/cars/searchbrands")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchBrands()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var brands = db.Cars.Where(p => p.Brand.Contains(term))
                        .Select(p => p.Brand).ToListAsync();
                return Ok(await brands);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("searchmodels")]
        [Route("api/cars/searchmodels")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchModels()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();

                var models = db.Cars.Where(p => p.Model.Contains(term))
                        .Select(p => p.Model).ToListAsync();
                return Ok(await models);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}