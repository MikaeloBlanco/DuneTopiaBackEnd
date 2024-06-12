using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.DTos;
using DunetopiaBackEnd.Models.Database.Entities;
using DunetopiaBackEnd.Models.DTos;
using Microsoft.AspNetCore.Mvc;

namespace DunetopiaBackEnd.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private MyDBContext _dbContextDunetopia;

    private const string PRODUCTOS_PATH = "images";

    public ProductoController(MyDBContext dbContextDunetopia)
    {
        _dbContextDunetopia = dbContextDunetopia;
    }

    [HttpGet("detalleProducto")]
    public IEnumerable<ProductoDto> GetProductoView()
    {
        return _dbContextDunetopia.Productos.Select(ToDto);
    }

    [HttpPost("crearProducto")]
    public async Task<IActionResult> Post([FromForm] CreateProducto productoDto)
    {
        if (productoDto.File == null)
        {
            return BadRequest("No se ha proporcionado ningun archivo.");
        }

        using Stream stream = productoDto.File.OpenReadStream();
        string productPath = $"{Guid.NewGuid()}_{productoDto.File.FileName}";
        string productoImage = await FileService.SaveAsync(stream, PRODUCTOS_PATH, productPath);

        Producto newProducto = new Producto()
        {
            Name = productoDto.Name,
            Description = productoDto.Description,
            Price = productoDto.Price,
            Stock = productoDto.Stock,
            Image = productoImage
        };

        await _dbContextDunetopia.Productos.AddAsync(newProducto);
        await _dbContextDunetopia.SaveChangesAsync();

        ProductoDto ProductoCreado = ToDto(newProducto);

        return Created($"/{newProducto.Id}", ProductoCreado);
    }
    [HttpPut("modifyProducto/{productoId}")]
    public async Task<IActionResult> ModifyProducto(int productoId, [FromForm] RefrescarProducto modifiedProductDto)
    {
        try
        {
            if (productoId <= 0)
            {
                return BadRequest("ID de producto no válido");
            }

            var productToUpdate = await _dbContextDunetopia.Productos.FindAsync(productoId);

            if (productToUpdate == null)
            {
                return NotFound($"Producto con ID {productoId} no encontrado");
            }
            productToUpdate.Name = modifiedProductDto.Nombre ?? productToUpdate.Name;
            productToUpdate.Description = modifiedProductDto.Descripcion ?? productToUpdate.Description;
            productToUpdate.Price = modifiedProductDto.Precio != 0 ? modifiedProductDto.Precio : productToUpdate.Price;
            productToUpdate.Stock = modifiedProductDto.Stock >= 0 ? modifiedProductDto.Stock : productToUpdate.Stock;

            if (modifiedProductDto.File != null)
            {
                using Stream stream = modifiedProductDto.File.OpenReadStream();
                string productPath = $"{Guid.NewGuid()}_{modifiedProductDto.File.FileName}";
                productToUpdate.Image = await FileService.SaveAsync(stream, PRODUCTOS_PATH, productPath);
            }
            await _dbContextDunetopia.SaveChangesAsync();

            return Ok(new { Message = "Producto modificado completamente " });
        }
        catch
        {
            return BadRequest(new { Error = "Error al modificar el producto" });
        }
    }

    private ProductoDto ToDto(Producto producto)
    {
        return new ProductoDto()
        {
            Id = producto.Id,
            Nombre = producto.Name,
            Descripcion = producto.Description,
            Price = producto.Price,
            Stock = producto.Stock,
            Image = producto.Image,
        };
    }
}

