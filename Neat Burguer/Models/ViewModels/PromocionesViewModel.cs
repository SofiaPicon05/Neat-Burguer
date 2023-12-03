namespace Neat_Burguer.Models.ViewModels
{
    public class PromocionesViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public decimal PrecioPromocion { get; set; }
        public int INDX { get; set; }
        public int INDF { get; set; }
    }
}
