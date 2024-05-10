namespace DunetopiaBackEnd.Models.Database.DTos;

    public class ProductoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock {  get; set; }
        public string image {  get; set; }

    }

