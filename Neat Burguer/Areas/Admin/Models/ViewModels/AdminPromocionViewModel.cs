namespace Neat_Burguer.Areas.Admin.Models.ViewModels
{
    public class AdminPromocionViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal? Precio { get; set; }
        public decimal? PrecioPromocion { get; set; }

    }
}
