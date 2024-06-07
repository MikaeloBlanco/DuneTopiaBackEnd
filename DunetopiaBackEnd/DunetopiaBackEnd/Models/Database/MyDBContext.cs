using Microsoft.EntityFrameworkCore;
using DunetopiaBackEnd.Models.Database.Entities;

namespace DunetopiaBackEnd.Models.Database;

public class MyDBContext : DbContext
{

    private const string DATBASE_PATH = "dunetopia.db";

    //tablas de datos
    public DbSet<Usuario> Usuarios {  get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<ProductoCarro> ProductoCarros { get; set; }
    public DbSet<ProductoPedido> ProductoPedidos { get; set; }
    public DbSet<CarroDeCompra> CarroDeCompras { get; set;}
    public DbSet<Compra> Compras { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        options.UseSqlite($"DataSource={baseDir}{DATBASE_PATH}");
    }

}
