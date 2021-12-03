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
    public class ComentarioController : ControllerBase
    {
        private readonly DBESANDWContext _context;

        public ComentarioController(DBESANDWContext context)
        {
            _context = context;
        }

        // GET: api/Comentario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comentario>>> GetComentario()
        {
            return await _context.Comentario.ToListAsync();
        }

        // GET: api/Comentario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comentario>> GetComentario(int id)
        {
            var comentario = await _context.Comentario.FindAsync(id);

            if (comentario == null)
            {
                return NotFound();
            }

            return comentario;
        }

        // PUT: api/Comentario/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComentario(int id, Comentario comentario)
        {
            if (id != comentario.Id)
            {
                return BadRequest();
            }

            _context.Entry(comentario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComentarioExists(id))
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

        // POST: api/Comentario
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WS_Response<Comentario>>> PostComentario(Comentario comentario)
        {
            WS_Response<Comentario> response = new WS_Response<Comentario>();
            Header header = new Header();
            _context.Comentario.Add(comentario);
            try
            {
                await _context.SaveChangesAsync();
                response.Objeto = comentario;
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = string.Empty;
            }
            catch (DbUpdateException e)
            {
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = e.Message;
            }

            response.Header = header;
            return Ok(response);
        }

        // DELETE: api/Comentario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.Comentario.FindAsync(id);
            if (comentario == null)
            {
                return NotFound();
            }

            _context.Comentario.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComentarioExists(int id)
        {
            return _context.Comentario.Any(e => e.Id == id);
        }
    }
}
