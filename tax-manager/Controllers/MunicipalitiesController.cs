using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tax_manager;
using tax_manager.model;

namespace tax_manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipalitiesController : ControllerBase
    {
        private readonly TaxManagerContext _context;

        public MunicipalitiesController(TaxManagerContext context)
        {
            _context = context;
        }

        // GET: api/Municipalities/test
        [HttpGet("test")]
        public async Task<ActionResult<Municipality>> GetTest()
        {
            return new Municipality("name", new List<Tax>(), new List<Tax>(), new List<Tax>(), new List<Tax>());
        }

        // GET: api/Municipalities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Municipality>>> GetMunicipalities()
        {
            return await _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .ToListAsync();
        }

        // GET: api/Municipalities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Municipality>> GetMunicipality(long id)
        {
            var municipality = await _context.Municipalities
                .Include(m => m.YearlyTaxes)
                .Include(m => m.MonthlyTaxes)
                .Include(m => m.WeeklyTaxes)
                .Include(m => m.DailyTaxes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (municipality == null)
            {
                return NotFound();
            }

            return municipality;
        }

        // PUT: api/Municipalities/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMunicipality(long id, Municipality municipality)
        {
            if (id != municipality.Id)
            {
                return BadRequest();
            }

            _context.Entry(municipality).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MunicipalityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Municipalities
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Municipality>> PostMunicipality(Municipality municipality)
        {
            _context.Municipalities.Add(municipality);
            await _context.SaveChangesAsync();
            return CreatedAtAction("PostMunicipality", new { id = municipality.Id }, municipality);
        }

        // DELETE: api/Municipalities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Municipality>> DeleteMunicipality(long id)
        {
            var municipality = await _context.Municipalities.FindAsync(id);
            if (municipality == null)
            {
                return NotFound();
            }

            _context.Municipalities.Remove(municipality);
            await _context.SaveChangesAsync();

            return municipality;
        }

        private bool MunicipalityExists(long id)
        {
            return _context.Municipalities.Any(e => e.Id == id);
        }
    }
}
