namespace DunetopiaBackEnd.Models.Database.Entities;

public class CarroDeCompra
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int ProductoCarroId { get; set; }

    //Clave foranea
    public ICollection<ProductoCarro> productoCarros { get; set; }
}

