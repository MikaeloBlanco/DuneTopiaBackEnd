using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DunetopiaBackEnd.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CompraController : ControllerBase
{
    private MyDBContext _dbContextDuneTopia;

    public CompraController(MyDBContext dbContextDuneTopia)
    {
        _dbContextDuneTopia = dbContextDuneTopia;
    }

    [HttpGet("historialDeProductos")]
    public IEnumerable<ProductoPedido> getHistorialProductos()
    {
        return _dbContextDuneTopia.ProductoPedidos;
    }
    [HttpGet("compra")]
    public IEnumerable<Compra> GetCompras()
    {
        return _dbContextDuneTopia.Compras;
    }
}
