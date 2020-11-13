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
    public class EstadoPedidosController : ControllerBase
    {
        private readonly DataContext _context;

        public EstadoPedidosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/EstadoPedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoPedido>>> GetEstadosPedidos()
        {
            return await _context.EstadosPedidos.ToListAsync();
        }

        // GET: api/EstadoPedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoPedido>> GetEstadoPedido(int id)
        {
            var estadoPedido = await _context.EstadosPedidos.FindAsync(id);

            if (estadoPedido == null)
            {
                return NotFound();
            }

            return estadoPedido;
        }

        // PUT: api/EstadoPedidos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstadoPedido(int id, EstadoPedido estadoPedido)
        {
            if (id != estadoPedido.Id)
            {
                return BadRequest();
            }

            _context.Entry(estadoPedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoPedidoExists(id))
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

        // POST: api/EstadoPedidos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EstadoPedido>> PostEstadoPedido(EstadoPedido estadoPedido)
        {
            _context.EstadosPedidos.Add(estadoPedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstadoPedido", new { id = estadoPedido.Id }, estadoPedido);
        }

        // DELETE: api/EstadoPedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EstadoPedido>> DeleteEstadoPedido(int id)
        {
            var estadoPedido = await _context.EstadosPedidos.FindAsync(id);
            if (estadoPedido == null)
            {
                return NotFound();
            }

            _context.EstadosPedidos.Remove(estadoPedido);
            await _context.SaveChangesAsync();

            return estadoPedido;
        }

        private bool EstadoPedidoExists(int id)
        {
            return _context.EstadosPedidos.Any(e => e.Id == id);
        }
    }
}
