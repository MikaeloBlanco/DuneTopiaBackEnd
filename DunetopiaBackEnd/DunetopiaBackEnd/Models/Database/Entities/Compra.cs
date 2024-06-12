namespace DunetopiaBackEnd.Models.Database.Entities;

public class Compra
{
    public int Id { get; set; }
    public string Valor { get; set; }
    public string ClientWallet { get; set; }
    public bool Completed { get; set; }
    public string Hash { get; set; }
    public int IdUsuario { get; set; }
    public decimal Precio { get; set; }
    public string fecha { get; set; }

}
