namespace DunetopiaBackEnd.Models.Database.Entities;

    public class CarroDeCompra
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CartProductId { get; set; }
    
    //Clave foranea
    public ICollection<ProductoCarro> ProductoCarro {  get; set; }
    }

