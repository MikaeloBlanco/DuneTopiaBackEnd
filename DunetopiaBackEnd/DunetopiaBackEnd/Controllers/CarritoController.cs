using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.Entities;
using Microsoft.AspNetCore.Mvc;
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
    [HttpPost("anadiracarrito")]
    public async Task<IActionResult> AddProducto([FromForm] int idProducto, [FromForm] int idUsuario, [FromForm] int cantidad)
    {
        var producto = await _dbContextDunetopia.ProductoCarros
            .FirstOrDefaultAsync(id => id.CarroDeCompraId == idUsuario && id.ProductoId == idProducto);
        if (producto != null)
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
                ProductoId = idProducto,
                CarroDeCompraId = idUsuario,
                Cantidad = cantidad
            };

            await _dbContextDunetopia.ProductoCarros.AddAsync(addProducto);
            await _dbContextDunetopia.SaveChangesAsync();

            return Created($"/{idProducto}", addProducto);
        }
    }
}

