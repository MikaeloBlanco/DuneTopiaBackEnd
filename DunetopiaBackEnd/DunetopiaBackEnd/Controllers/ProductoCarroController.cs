using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DunetopiaBackEnd.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductoCarroController : ControllerBase
{
    private MyDBContext _dbContextDunetopia;
    public ProductoCarroController(MyDBContext dbContextDunetopia)
    {
        _dbContextDunetopia = dbContextDunetopia;
    }

    [HttpGet("productoscarro")]
    public IEnumerable<ProductoCarro> GetProductosCarrito()
    {
        return _dbContextDunetopia.ProductoCarros;
    }
    [HttpPut("eliminarProducto")]
    public async Task<IActionResult> DeleteProducto([FromForm] int productoId, [FromForm] int usuarioId)
    {
        var ListaProducto = await _dbContextDunetopia.ProductoCarros
            .FirstOrDefaultAsync(id => id.CarroDeCompraId == usuarioId && id.ProductoId == productoId);
        _dbContextDunetopia.ProductoCarros.Remove(ListaProducto);
        await _dbContextDunetopia.SaveChangesAsync();
        return Ok("Producto Eliminado");
    }
    [HttpPut("cambiarCantidad")]
    public async Task<IActionResult> ModifyProducto([FromForm] int productoId, [FromForm] int usuarioId, [FromForm] int cantidad)
    {
        var producto = await _dbContextDunetopia.ProductoCarros
            .FirstOrDefaultAsync(id => id.CarroDeCompraId == usuarioId && id.ProductoId == productoId);
        if (producto != null)
        {
            producto.Cantidad = cantidad;
            _dbContextDunetopia.ProductoCarros.Update(producto);
            await _dbContextDunetopia.SaveChangesAsync();
            return Ok("Producto actualizado");
        }
        else
        {
            return NotFound("Producto no encontrado en el carrito");
        }
    } 
}
