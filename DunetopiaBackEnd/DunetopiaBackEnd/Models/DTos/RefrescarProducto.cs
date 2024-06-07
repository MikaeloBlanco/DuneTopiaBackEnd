namespace DunetopiaBackEnd.Models.DTos
{
    public class RefrescarProducto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public IFormFile File { get; set; }
    }
}
