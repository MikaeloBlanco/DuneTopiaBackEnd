using Microsoft.EntityFrameworkCore;
namespace DunetopiaBackEnd.Models.Database.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Usuario
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Direccion { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

    public ICollection<Pedido> Pedidos { get; set; }
}

