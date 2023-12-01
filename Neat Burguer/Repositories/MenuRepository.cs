using Neat_Burguer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Neat_Burguer.Repositories
{
    public class MenuRepository : Repository<Menu>
    {
        public MenuRepository(NeatContext ctx) : base(ctx)
        {

        }
        public override IEnumerable<Menu> GetAll()
        {
            return Ctx.Menu
                .Include(x => x.IdClasificacionNavigation)
                .OrderBy(x => x.Nombre);
        }

        public Menu? GetByName(string nombre)
        {
            return Ctx.Menu
                .Include(x=> x.IdClasificacionNavigation)
                .FirstOrDefault(x=> x.Nombre == nombre);
                
        }
    }
}
