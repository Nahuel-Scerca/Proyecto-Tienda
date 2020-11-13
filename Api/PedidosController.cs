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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosDisponibles()
        {
            var usuario = User.Identity.Name;
            if (usuario != null)
            {
                var pedidos = await _context.Pedidos.Where(pedidos=> pedidos.Asingnado==0).ToListAsync();
                return pedidos;
            }
            return NotFound();

        }

        // GET: api/Pedidos/5
        [HttpGet("/PedidosAdquiridos")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedido()
        {
            var usuario = User.Identity.Name;

            if (usuario != null)
            {
                var pedidos = await _context.Pedidos.Where(pedidos => pedidos.UsuarioACargo == usuario).ToListAsync();
                return pedidos;
            }
            return NotFound();
        }

        // PUT: api/Pedidos/5
        [HttpPut("/ActualizarEstado/{id}")]
        public async Task<IActionResult> PutPedido(int id, int estadoPedidoId)
        {
            try
            {
                var pedido = await _context.Pedidos.FirstOrDefaultAsync(e=> e.Id ==id);
                if (pedido != null)
                {
                    if (pedido.UsuarioACargo == User.Identity.Name)
                    {
                        pedido.EstadoPedidoId = estadoPedidoId;
                        _context.Pedidos.Update(pedido);
                        _context.SaveChanges();
                        return Ok();
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
