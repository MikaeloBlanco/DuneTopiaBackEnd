namespace DunetopiaBackEnd.Models.Database.DTos;

public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Image { get; set; }

}

