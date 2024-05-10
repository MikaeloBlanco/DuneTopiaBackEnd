namespace DunetopiaBackEnd.Models.Database.Entities;

    public class Pedido
    {
       public int Id { get; set; }
       public string TotalPrice {  get; set; }
       public bool Status { get; set; }
       public DateTime Date {  get; set; }
       public int UsuarioId { get; set; }
       public int ProductoCarro {  get; set; }

        //Claves Foraneas
        public ICollection<Usuario> Usuarios { get; set; }
        public ICollection<ProductoCarro> Productos { get; set; }
    }

