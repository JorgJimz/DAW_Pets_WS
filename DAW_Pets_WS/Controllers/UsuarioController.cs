﻿using System.Collections.Generic;
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
    public class UsuarioController : ControllerBase
    {
        private readonly DBESANDWContext _context;

        public UsuarioController(DBESANDWContext context)
        {
            _context = context;
        }

        [HttpGet("login/{user}/{pwd}")]
        public async Task<ActionResult<Usuario>> Login(string user, string pwd)
        {
            var usuario = await _context.Usuario.Include(x => x.Persona).Include(y => y.Rol).Where(x => x.Login.Equals(user)).SingleOrDefaultAsync();
            if (HashHelper.CheckHash(pwd, usuario.Password, usuario.Sal))
            {
                return usuario;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            return await _context.Usuario.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<WS_Response<Usuario>>> PutUsuario(int id, Usuario usuario)
        {
            WS_Response<Usuario> response = new WS_Response<Usuario>();
            Header header = new Header();
            if (id != usuario.PersonaId)
            {
                return BadRequest();
            }
            Usuario uModified = _context.Usuario.Where(x => x.PersonaId == usuario.PersonaId).FirstOrDefaultAsync().Result;
            uModified.Estado = 1;
            _context.Entry(uModified).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                response.Objeto = usuario;
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
    }
}