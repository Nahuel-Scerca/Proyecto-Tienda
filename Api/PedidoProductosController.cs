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
    public class PedidoProductosController : ControllerBase
    {
        private readonly DataContext _context;

        public PedidoProductosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PedidoProductos
        [HttpGet("{pedidoId}")]
        public async Task<ActionResult<IEnumerable<PedidoProducto>>> GetPedidosProductos(int pedidoId)
        {
            try
            {
                var usuario = User.Identity.Name;
                if (usuario != null)
                {

                    var pedidosProducto = await _context.PedidosProductos
                        .Include(pedidosProducto => pedidosProducto.Producto)
                        .Include(pedidosProducto => pedidosProducto.Pedido)
                        .Where(pedidosProducto => pedidosProducto.PedidoId == pedidoId).ToListAsync();
                    return pedidosProducto;
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // PUT: api/PedidoProductos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedidoProducto(int id, PedidoProducto pedidoProducto)
        {
            if (id != pedidoProducto.Id)
            {
                return BadRequest();
            }

            _context.Entry(pedidoProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoProductoExists(id))
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

        // POST: api/PedidoProductos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PedidoProducto>> PostPedidoProducto(PedidoProducto pedidoProducto)
        {
            _context.PedidosProductos.Add(pedidoProducto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedidoProducto", new { id = pedidoProducto.Id }, pedidoProducto);
        }

        // DELETE: api/PedidoProductos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PedidoProducto>> DeletePedidoProducto(int id)
        {
            var pedidoProducto = await _context.PedidosProductos.FindAsync(id);
            if (pedidoProducto == null)
            {
                return NotFound();
            }

            _context.PedidosProductos.Remove(pedidoProducto);
            await _context.SaveChangesAsync();

            return pedidoProducto;
        }

        private bool PedidoProductoExists(int id)
        {
            return _context.PedidosProductos.Any(e => e.Id == id);
        }
    }
}
