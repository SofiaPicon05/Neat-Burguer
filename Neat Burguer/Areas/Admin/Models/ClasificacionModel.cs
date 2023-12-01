namespace Neat_Burguer.Areas.Admin.Models
{
    public class ClasificacionModel
    {

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public IEnumerable<MenuModel> Menus { get; set; } = Enumerable.Empty<MenuModel>();
    }
}
