﻿namespace Neat_Burguer.Areas.Admin.Models.ViewModels
{
    public class QuitarPromocionViewModel
    {
        public int IdMenu { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal PrecioOriginal { get; set; }
        public decimal PrecioNuevo { get; set; }
    }
}
