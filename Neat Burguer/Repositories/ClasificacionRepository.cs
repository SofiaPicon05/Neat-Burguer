using Microsoft.EntityFrameworkCore;
using Neat_Burguer.Models.Entities;

namespace Neat_Burguer.Repositories
{
    public class ClasificacionRepository
    {
        public ClasificacionRepository(NeatContext ctx) 
        {
            Ctx = ctx;
        }
        public NeatContext Ctx { get; }
        public IEnumerable<Clasificacion>GetAll()
        {
            return Ctx.Clasificacion
                .Include(x => x.Menu)
                .OrderBy(x => x.Nombre);
        }

    }
}
