using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DunetopiaBackEnd.Controllers;
[Route("api/[controller]")]
[ApiController]

    public class CarritoController : ControllerBase
    {

        private MyDBContext _dbContextDunetopia;

        public CarritoController(MyDBContext dbContextDunetopia)
        {
            _dbContextDunetopia = dbContextDunetopia;
        }

        [HttpGet("Carrito")]
        public IEnumerable<CarroDeCompra> GetCarroDeComprasView()
        {
            return _dbContextDunetopia.CarroDeCompras;
        }
        [HttpPost("añadiracarrito")]
        public async Task<IActionResult> addProducto([FromForm]int productoId, [FromForm] int usuarioId, [FromForm] int cantidad)
        {
            var producto = await _dbContextDunetopia.ProductoCarros
                .FirstOrDefaultAsync(id => id.CarroDeCompraId == usuarioId && id.ProductoId == productoId);
            if (producto == null)
            {
                producto.Cantidad += cantidad;
                _dbContextDunetopia.ProductoCarros.Update(producto);
                await _dbContextDunetopia.SaveChangesAsync();
                return Ok("Producto Actualizado");
            }
            else
            {
                ProductoCarro addProducto = new ProductoCarro()
                {
                    ProductoId = productoId,
                    CarroDeCompraId = usuarioId,
                    Cantidad = cantidad
                };

                await _dbContextDunetopia.ProductoCarros.AddAsync(addProducto);
                await _dbContextDunetopia.SaveChangesAsync();

                return Created($"/{productoId}", addProducto);
            }
        }
    }

