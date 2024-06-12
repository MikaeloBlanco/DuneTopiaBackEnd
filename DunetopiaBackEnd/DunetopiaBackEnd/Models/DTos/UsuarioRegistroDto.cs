namespace DunetopiaBackEnd.Models.DTos;

public class UsuarioRegistroDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Direccion { get; set; }
    public bool isAdmin { get; set; }
}
