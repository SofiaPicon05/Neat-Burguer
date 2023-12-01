﻿namespace Neat_Burguer.Areas.Admin.Models.ViewModels
{
    public class EditarMenuViewModel
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdClasificacion { get; set; }
        public IEnumerable<ClasificacionModel> Clasificaciones { get; set; } = Enumerable.Empty<ClasificacionModel>();
        public IFormFile? Archivo { get; set; }
    }
}
