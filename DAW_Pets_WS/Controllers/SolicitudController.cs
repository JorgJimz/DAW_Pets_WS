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
    public class SolicitudController : ControllerBase
    {
        private readonly DBESANDWContext _context;

        public SolicitudController(DBESANDWContext context)
        {
            _context = context;
        }

        // GET: api/Solicitud
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Solicitud>>> GetSolicitud()
        {
            return await _context.Solicitud.Include("Usuario.Persona").Include("Mascota").Include("EstadoNavigation").ToListAsync();
        }

        // GET: api/Solicitud/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Solicitud>> GetSolicitud(int id)
        {
            var solicitud = await _context.Solicitud.Include("Usuario.Persona").Include("Mascota").Include("EstadoNavigation").Where(x => x.Id == id).FirstOrDefaultAsync();

            if (solicitud == null)
            {
                return NotFound();
            }

            return solicitud;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WS_Response<Solicitud>>> PutSolicitud(Solicitud solicitud)
        {
            WS_Response<Solicitud> response = new WS_Response<Solicitud>();
            Header header = new Header();

            Solicitud sModified = _context.Solicitud.Where(x => x.Id == solicitud.Id).FirstOrDefaultAsync().Result;
            sModified.Estado = solicitud.Estado;
            sModified.Detalle = solicitud.Detalle;
            _context.Entry(sModified).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                response.Objeto = sModified;
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = "Usuario activado.";
            }
            catch (DbUpdateConcurrencyException e)
            {
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = e.Message;
            }

            response.Header = header;
            return Ok(response);
        }

        // POST: api/Solicitud
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Solicitud>> PostSolicitud(Solicitud solicitud)
        {
            WS_Response<Solicitud> response = new WS_Response<Solicitud>();
            Header header = new Header();

            try
            {
                _context.Solicitud.Add(solicitud);
                await _context.SaveChangesAsync();
                response.Objeto = solicitud;
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = "Solicitud ingresada exitosamente.";
            }
            catch (Exception e)
            {
                header.CodigoRetorno = HeaderEnum.Incorrecto.ToString();
                header.DescRetorno = e.Message;
            }

            response.Header = header;
            return Ok(response);
        }

        // DELETE: api/Solicitud/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitud(int id)
        {
            var solicitud = await _context.Solicitud.FindAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }

            _context.Solicitud.Remove(solicitud);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicitud.Any(e => e.Id == id);
        }
    }
}
