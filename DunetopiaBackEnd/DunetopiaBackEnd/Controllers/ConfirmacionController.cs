using DunetopiaBackEnd.Models.Database;
using DunetopiaBackEnd.Models.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DunetopiaBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmacionController : ControllerBase
    {
        private readonly MyDBContext _DuneTopiaDataBase;


        public ConfirmacionController(MyDBContext duneTopiaDataBase)
        {
            _DuneTopiaDataBase = duneTopiaDataBase;
        }
        [HttpPost("ComprarProducto")]
        public async Task<Compracion> CompraAsync([FromForm] decimal totalPrecio, [FromForm] int idUsuario)
        {
            ProductoCarro productoCarro = _DuneTopiaDataBase.ProductoCarros
                .FirstOrDefault(id => id.CarroDeCompraId == idUsuario);

            Compracion compracion = new Compracion()
            {
                Valor = totalPrecio.ToString()
            };

            Compra compra = new Compra()
            {

                IdUsuario = idUsuario,
                Precio = totalPrecio,
                fecha = DateTime.Now.ToString("dd MM yyyy")
            };

            await _DuneTopiaDataBase.Compras.AddAsync(compra);
            await _DuneTopiaDataBase.SaveChangesAsync();
            compracion.Id = compra.Id;

            return compracion;
        }

        [HttpPost("check/{compraID}")]
        public async Task<bool> CheckCompraAsync(int compraId)
        {
            bool success = true;
            Compra compra = await _DuneTopiaDataBase.Compras.FirstOrDefaultAsync(id => id.Id == compraId);
            Console.WriteLine(compra);

            if (success)
            {
                ProductoCarro[] productoCarros2 = _DuneTopiaDataBase.ProductoCarros.Where(p => p.CarroDeCompraId == compra.IdUsuario).ToArray();
                ProductoCarro[] productoCarros = [.. _DuneTopiaDataBase.ProductoCarros.Where(p => p.CarroDeCompraId == compra.IdUsuario)];

                List<Producto> productos = new List<Producto>();

                Console.WriteLine(productoCarros2.Length);
                Console.WriteLine(productoCarros.Length);

                for (int i = 0; i < productoCarros.Length; i++)
                {
                    Console.WriteLine("i:" + i);
                    ProductoPedido nuevoProductoPedido = new ProductoPedido()
                    {
                        ProductoId = productoCarros[i].Id,
                        Cantidad = productoCarros[i].Cantidad,
                        PedidosId = compra.Id.ToString()
                    };

                    var productoModificado = _DuneTopiaDataBase.Productos.FirstOrDefault(p => p.Id == productoCarros[i].ProductoId);
                    productoModificado.Stock -= productoCarros[i].Cantidad;
                    _DuneTopiaDataBase.Productos.Update(productoModificado);

                    Console.WriteLine("ID de Pedido: " + nuevoProductoPedido.PedidosId);
                    await _DuneTopiaDataBase.ProductoPedidos.AddAsync(nuevoProductoPedido);
                }
                for (int i = 0; i < productoCarros.Length; i++)
                {
                    _DuneTopiaDataBase.ProductoCarros.Remove(_DuneTopiaDataBase.ProductoCarros.FirstOrDefault(p => p.CarroDeCompraId == compra.IdUsuario));
                    await _DuneTopiaDataBase.SaveChangesAsync();
                }
            }
            compra.Completed = success;
            await _DuneTopiaDataBase.SaveChangesAsync();

            return success;
        }
    }
}
