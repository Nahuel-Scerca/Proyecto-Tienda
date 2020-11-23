using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda_MAWS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Tienda_MAWS.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly DataContext _context;

        public PedidosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Pedidos
        [HttpGet("Disponibles")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosDisponibles()
        {
            try
            {
                var usuario = User.Identity.Name;
                if (usuario != null)
                {

                    var pedidos = await _context.Pedidos
                        .Include(pedidos => pedidos.Cliente)
                        .Where(pedidos => pedidos.Asignado == 0).ToListAsync();
                    return pedidos;
                }
                return NotFound();
            }
            catch(Exception ex){
                return BadRequest(ex);
            }
          
        }


        // GET: api/Pedidos/Vendidos
        [HttpGet("Vendidos")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosVendidos()
        {
            try
            {
                var usuario = User.Identity.Name;
                if (usuario != null)
                {

                    var pedidos = await _context.Pedidos
                        .Include(pedidos => pedidos.Cliente)
                        .Where(pedidos => pedidos.Estado == 3).ToListAsync();
                    return pedidos;
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // GET: api/Pedidos/Adquiridos
        [HttpGet("Adquiridos")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedido()
        {
            try {
                var usuario = User.Identity.Name;

                if (usuario != null)
                {
                    var pedidos = await _context.Pedidos
                        .Include(pedidos => pedidos.Cliente)
                        .Where(pedidos => pedidos.UsuarioACargo == usuario && pedidos.Estado!= 3).ToListAsync();
                    return pedidos;
                }
                return NotFound();
            }
            catch(Exception ex) {
                return BadRequest(ex);
            }
          
        }
        // PUT: api/Pedidos/{id}
        [HttpPut("AdquirirPedido/{id}")]
        public async Task<IActionResult> PutAdquirirPedido(int id)
        {
            try
            {
                var usuario = User.Identity.Name;

                if (usuario != null)
                {
                    var pedido = await _context.Pedidos.FirstOrDefaultAsync(e => e.Id == id);
                    if (pedido != null)
                    {
                        
                        pedido.Asignado = 1;
                        pedido.UsuarioACargo = usuario;
                        _context.Pedidos.Update(pedido);
                        _context.SaveChanges();

                        return Ok();
                        

                    }
                    return BadRequest();
                }
                else 
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // PUT: api/Pedidos/ActualizarEstado/{id}
        [HttpPut("ActualizarEstado")]
        public async Task<IActionResult> PutPedido(Pedido pedido)
        {
            try
            {
              
                if (pedido != null)
                {
                    if (pedido.UsuarioACargo == User.Identity.Name)
                    {
                        _context.Pedidos.Update(pedido);
                        _context.SaveChanges();
                        return Ok(pedido);
                    }
                    
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.Id }, pedido);
        }

        // DELETE: api/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pedido>> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        private bool PedidoExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
