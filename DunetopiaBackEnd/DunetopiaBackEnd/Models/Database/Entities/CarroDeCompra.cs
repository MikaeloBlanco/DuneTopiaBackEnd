namespace DunetopiaBackEnd.Models.Database.Entities;

public class CarroDeCompra
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CartProductId { get; set; }

    //Clave foranea
    public ICollection<CarroDeCompra> CarroDeCompras { get; set; }
    public ICollection<Producto> productos { get; set; }
}

