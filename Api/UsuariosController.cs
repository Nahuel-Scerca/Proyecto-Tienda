using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Tienda_MAWS.Controllers;

namespace Tienda_MAWS.Models
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext contexto;
        private readonly IConfiguration config;

        public UsuariosController(DataContext context, IConfiguration config)
        {
            this.contexto = context;
            this.config = config;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<IActionResult> GetUsuario()
        {
            try
            {
                var usuario = User.Identity.Name;

                var res = contexto.Usuarios.SingleOrDefault(x => x.Email == usuario);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Usuarios/Ventas
        [HttpGet("Todos")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetVentasUsuario()
        {
            try
            {
                
                var res = await  contexto.Usuarios.ToListAsync();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Usuarios/CantidadVentas
        [HttpGet("CantidadVentas")]
        public async Task<ActionResult<IEnumerable<UsuarioVentas>>> GetVentasYUsuario()
        {
            try
            {
                var usuarios  = await contexto.Usuarios.ToListAsync();
                var usuarioVentas = new List<UsuarioVentas>();
                foreach(Usuario usuario in usuarios)
                {
                    var cantidadVentas = await contexto.Pedidos.Where(pedido => pedido.UsuarioACargo == usuario.Email && pedido.Estado == 3).CountAsync();
                    
                    UsuarioVentas uv = new UsuarioVentas{
                    Usuario=usuario,CantidadVentas=cantidadVentas
                    };
                    usuarioVentas.Add(uv);
                }
                
                return Ok(usuarioVentas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // PUT: api/Usuarios/5    
        [HttpPut]
        public async Task<IActionResult> PutUsuario([FromBody]Usuario user)
        {
            var usuario = User.Identity.Name;

            try
            {
                if (contexto.Usuarios.AsNoTracking().FirstOrDefault(user => user.Email == User.Identity.Name) != null)
                {
                    contexto.Usuarios.Update(user);
                    contexto.SaveChanges();
                    return Ok(user);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            //contexto.Usuarios.Add(usuario);
            await contexto.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> DeleteUsuario(int id)
        {
            var usuario = await contexto.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            if (usuario.Email == User.Identity.Name)
            {
                contexto.Usuarios.Remove(usuario);
                await contexto.SaveChangesAsync();

                return usuario;
            }
            else
            {
                return Unauthorized();
            }
            
        }

        private bool UsuarioExists(int id)
        {
            return contexto.Usuarios.Any(e => e.Id == id);
        }


        // GET api/<controller>/5
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var u = contexto.Usuarios.FirstOrDefault(x => x.Email == loginView.Usuario);
                if (u == null || u.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, u.Email),
                        new Claim("FullName", u.Nombre + " " + u.Apellido),
                        new Claim(ClaimTypes.Role, "Empleado"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
