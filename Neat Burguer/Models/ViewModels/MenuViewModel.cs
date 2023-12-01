using System.Reflection.Metadata;

namespace Neat_Burguer.Models.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = null!;
    }

    public class HamburguesaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public string Descripcion { get; set; } = null!;
        public bool Seleccionado { get; set; } 
    }

    public class ClasificacionModel
    {
       
        public string Nombre { get; set; } = null!;
        public IEnumerable<HamburguesaModel> Hamburguesas { get; set; } = null!;
    }
    public class PromocionesViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } =null!;
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioNuevo { get; set; }
        public string SiguientePromocion { get; set; } = null!;
        public string AnteriorPromocion { get; set; } = null!;

    }
}
