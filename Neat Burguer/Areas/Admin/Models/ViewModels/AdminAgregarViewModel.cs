using Neat_Burguer.Models.Entities;

namespace Neat_Burguer.Areas.Admin.Models.ViewModels
{
    public class AdminAgregarViewModel
    {
        public Menu menu { get; set; } = new();
        public IEnumerable<Clasificacion> clasificaciones { get; set; } = null!;
        public IFormFile? Archivo { get; set; }
    }
}
