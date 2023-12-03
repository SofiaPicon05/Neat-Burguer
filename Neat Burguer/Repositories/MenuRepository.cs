using Neat_Burguer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Neat_Burguer.Repositories
{
    public class MenuRepository 
    {
        public NeatContext Context { get; }

        public MenuRepository(NeatContext context)
        {
            Context = context;
        }
        public IEnumerable<Menu> GetAll()
        {
            return Context.Menu.OrderBy(x => x.Nombre);
        }
        public Menu? GetById(int Id)
        {
            return Context.Menu.Find(Id);
        }
        public IEnumerable<Menu> GetMenuByNombre(string hamburguesa)
        {
            return Context.Menu
                .Where(x => x.Nombre == hamburguesa)
                .OrderBy(x => x.Nombre);
        }
        public Menu? GetByNombre(string nombre)
        {
            return Context.Menu.FirstOrDefault(x => x.Nombre == nombre);
        }
        public void Insert(Menu h)
        {
            Context.Add(h);
            Context.SaveChanges();
        }
        public void Update(Menu h)
        {
            Context.Update(h);
            Context.SaveChanges();
        }
        public void Delete(Menu h)
        {
            Context.Remove(h);
            Context.SaveChanges();
        }

    }
}
