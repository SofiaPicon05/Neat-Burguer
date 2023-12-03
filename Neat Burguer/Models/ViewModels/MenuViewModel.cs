using System.Reflection.Metadata;
using Neat_Burguer.Models.Entities;

namespace Neat_Burguer.Models.ViewModels
{
    public class MenuViewModel
    {
        public string Nombre { get; set; } = null!;
        public IEnumerable<ClasificaciónModel> ListaClasificacion { get; set; } = null!;
        public HamburguesaModel Hamburguesa { get; set; } = null!;
    }
   
    public class HamburguesaModel
    {
        public int Id { get; set; }
        public string Descripción { get; set; } = null!;
    }
    
    public class ClasificaciónModel
    {
        public string Nombre { get; set; } = null!;
        public IEnumerable<MenuModel> ListaMenu { get; set; } = null!;
    }
   
    public class MenuModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public bool Seleccionado { get; set; }
    }

}
