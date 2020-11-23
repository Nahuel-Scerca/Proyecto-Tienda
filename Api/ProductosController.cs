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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Tienda_MAWS.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHostingEnvironment environment;

        public ProductosController(DataContext context, IHostingEnvironment enviroment)
        {
            _context = context;
            this.environment = enviroment;
        }

        // GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            try
            {
                var productos = await _context.Productos.ToListAsync();
                return Ok(productos);
            }
            catch
            {
                return NotFound();
            }
        }

        // GET: api/Productos/{buscar}
        [HttpGet("{buscar}")]
        public async Task<ActionResult<List<Producto>>> GetProductoList(String buscar)
        {
            try
            {
                var usuario = User.Identity.Name;
                if (usuario != null)
                {
                    var productos = await _context.Productos
                        .Where(productos => productos.Nombre.Contains(buscar)).ToListAsync();
                    return productos;
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Productos/5
        [HttpGet("Unico/{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                if (usuario != null)
                {
                    var producto = await _context.Productos.FindAsync(id);
                    return producto;
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        // PUT: api/Productos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutProducto(Producto producto)
        {
            
            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(producto);
        }

        
        [HttpPost("Foto")]
        public async Task<ActionResult<Producto>> PostFotoProducto([FromForm] IFormFile file)
        {
            if (file != null)
            {
                string root = environment.WebRootPath;
                string path = Path.Combine(root, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }
                string fileName = file.FileName;
                string pathCompleto = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return Ok("Foto subida satisfactoriamente");
        }





        [HttpPost("Post")]
        public async Task<ActionResult<Producto>> PostProducto([FromBody]Producto producto)
        {
            try {
            
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return Ok(producto);
            }
            catch(Exception ex) {
                return BadRequest(ex);
            }

        }


        // DELETE: api/Productos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return producto;
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
