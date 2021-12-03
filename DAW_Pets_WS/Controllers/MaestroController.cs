using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAW_Pets_WS.Models;

namespace DAW_Pets_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaestroController : ControllerBase
    {
        private readonly DBESANDWContext _context;

        public MaestroController(DBESANDWContext context)
        {
            _context = context;
        }

        // GET: api/Maestro
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maestro>>> GetMaestro()
        {
            return await _context.Maestro.ToListAsync();
        }

        // GET: api/Maestro/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Maestro>> GetMaestro(int id)
        {
            var maestro = await _context.Maestro.FindAsync(id);

            if (maestro == null)
            {
                return NotFound();
            }

            return maestro;
        }

        // PUT: api/Maestro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaestro(int id, Maestro maestro)
        {
            if (id != maestro.Id)
            {
                return BadRequest();
            }

            _context.Entry(maestro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaestroExists(id))
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

        // POST: api/Maestro
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Maestro>> PostMaestro(Maestro maestro)
        {
            _context.Maestro.Add(maestro);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaestro", new { id = maestro.Id }, maestro);
        }

        // DELETE: api/Maestro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaestro(int id)
        {
            var maestro = await _context.Maestro.FindAsync(id);
            if (maestro == null)
            {
                return NotFound();
            }

            _context.Maestro.Remove(maestro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaestroExists(int id)
        {
            return _context.Maestro.Any(e => e.Id == id);
        }
    }
}
