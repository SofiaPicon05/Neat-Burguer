using System;
using System.Collections.Generic;

namespace Neat_Burguer.Models.Entities;

public partial class Clasificacion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Menu> Menu { get; set; } = new List<Menu>();
}
