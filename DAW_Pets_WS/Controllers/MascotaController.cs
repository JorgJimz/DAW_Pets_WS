using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAW_Pets_WS.Models;
using DAW_Pets_WS.Models.Helpers;

namespace DAW_Pets_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MascotaController : ControllerBase
    {
        private readonly DBESANDWContext _context;

        public MascotaController(DBESANDWContext context)
        {
            _context = context;
        }

        // GET: api/Mascota
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mascota>>> GetMascota()
        {
            return await _context.Mascota.ToListAsync();
        }

        // GET: api/Mascota/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mascota>> GetMascota(int id)
        {
            var mascota = await _context.Mascota.Include("Vacuna.VacunaNavigation").Include("Comentario.Usuario.Persona").FirstOrDefaultAsync(x => x.Id == id);

            if (mascota == null)
            {
                return NotFound();
            }

            return mascota;
        }

        // PUT: api/Mascota/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMascota(int id, Mascota mascota)
        {
            if (id != mascota.Id)
            {
                return BadRequest();
            }

            _context.Entry(mascota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MascotaExists(id))
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

        // POST: api/Mascota
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mascota>> PostMascota(Mascota mascota)
        {

            WS_Response<Mascota> response = new WS_Response<Mascota>();
            Header header = new Header();
            try
            {
                _context.Mascota.Add(mascota);
                await _context.SaveChangesAsync();

                response.Objeto = mascota;
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = "Mascota registrada.";
            }
            catch (Exception e)
            {
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = e.Message;
            }

            response.Header = header;
            return Ok(response);
        }

        // DELETE: api/Mascota/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _context.Mascota.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }

            _context.Mascota.Remove(mascota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascota.Any(e => e.Id == id);
        }
    }
}
