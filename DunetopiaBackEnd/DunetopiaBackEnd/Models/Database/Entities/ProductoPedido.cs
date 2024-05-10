namespace DunetopiaBackEnd.Models.Database.Entities;

    public class ProductoPedido
    {
       public int Id { get; set; }
       public int Qunatity { get; set; }
       public int ProductoId { get; set; }
       public string PedidosId {  get; set; }

      //Claves Foraneas
       public ICollection<Pedido> Pedidos { get; set; }
       public ICollection<Producto> Productos { get; set; }
    }

