using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAW_Pets_WS.Models;
using DAW_Pets_WS.Models.Helpers;
namespace DAW_Pets_WS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly DBESANDWContext _context;

        public PersonaController(DBESANDWContext context)
        {
            _context = context;
        }

        // GET: api/Persona
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersona()
        {
            return await _context.Persona.Include(x => x.Usuario).ToListAsync();
        }

        // GET: api/Persona/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(int id)
        {
            var persona = await _context.Persona.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return persona;
        }

        // PUT: api/Persona/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersona(int id, Persona persona)
        {
            if (id != persona.Id)
            {
                return BadRequest();
            }

            _context.Entry(persona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(id))
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

        // POST: api/Persona
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WS_Response<Persona>>> PostPersona(Persona persona)
        {
            WS_Response<Persona> response = new WS_Response<Persona>();
            Header header = new Header();
            try
            {
                _context.Persona.Add(persona);
                await _context.SaveChangesAsync();
                HashedPassword hashed = HashHelper.Hash(persona.Pwd);
                Usuario user = new Usuario()
                {
                    Login = persona.Email,
                    Password = hashed.Password,
                    Sal = hashed.Salt,
                    PersonaId = persona.Id,
                    RolId = 1,
                    Estado = persona.Estado
                };
                _context.Usuario.Add(user);
                await _context.SaveChangesAsync();
                response.Objeto = persona;
                header.CodigoRetorno = HeaderEnum.Correcto.ToString();
                header.DescRetorno = (user.Estado == 0) ? string.Format("Gracias por registrarte {0}, por favor, espera la aprobación del administrador.", persona.Nombre) : "Usuario registrado.";
            }
            catch (Exception e)
            {
                header.CodigoRetorno = HeaderEnum.Incorrecto.ToString();
                header.DescRetorno = e.Message;
            }

            response.Header = header;
            return Ok(response);
        }

        // DELETE: api/Persona/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersona(int id)
        {
            var persona = await _context.Persona.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            _context.Persona.Remove(persona);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.Id == id);
        }
    }
}
