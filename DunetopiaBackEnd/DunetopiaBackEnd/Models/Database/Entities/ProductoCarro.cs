namespace DunetopiaBackEnd.Models.Database.Entities;

public class ProductoCarro
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public int CarroDeCompraId { get; set; }
    public int Cantidad { get; set; }

    //Claves Foraneas
    public CarroDeCompra CarroDeCompras { get; set; }
    public Producto Productos { get; set; }
}
