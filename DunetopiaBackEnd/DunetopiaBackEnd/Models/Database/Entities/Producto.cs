using Microsoft.EntityFrameworkCore;

namespace DunetopiaBackEnd.Models.Database.Entities;

    [Index(nameof(Id), IsUnique = true)]
    public class Producto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string Image {  get; set; }
        public string Type { get; set; }
        public int ProductoCarroId { get; set; }


    //claves foraenas
    public ICollection<ProductoCarro> ProductoCarro { get; set; }
    //public ICollection<>  { get; set; }
    }

