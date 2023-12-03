using Microsoft.EntityFrameworkCore;
using Neat_Burguer.Models.Entities;

namespace Neat_Burguer.Repositories
{
    public class ClasificacionRepository
    {
      public NeatContext Context { get; set; }
        public ClasificacionRepository(NeatContext context)
        {
            Context = context;
        }
        public IEnumerable<Clasificacion> GetAll()
        {
            return Context.Clasificacion
                .Include(x => x.Menu)
                .OrderBy(x => x.Nombre);
        }
        public IEnumerable<Clasificacion> GetMenuByClasificacion(string clasificacion)
        {
            return Context.Clasificacion
                .Where(x => x.Menu != null && x.Nombre == clasificacion)
                .OrderBy(x => x.Nombre);
        }

        public Clasificacion? GetByName(string nombre)
        {
            return Context.Clasificacion
                .FirstOrDefault(x => x.Nombre == nombre);
        }
        public void Insert(Clasificacion h)
        {
            Context.Add(h);
            Context.SaveChanges();
        }
        public void Update(Clasificacion h)
        {
            Context.Update(h);
            Context.SaveChanges();
        }
        public void Delete(Clasificacion h)
        {
            Context.Remove(h);
            Context.SaveChanges();
        }
    }
}
