using Neat_Burguer.Models.Entities;

namespace Neat_Burguer.Repositories
{
    public class Repository <T> where T : class
    {
        public Repository(NeatContext ctx)
        {
            Ctx = ctx;
        }
        public NeatContext Ctx { get; }
        public virtual IEnumerable<T> GetAll()
        {
            return Ctx.Set<T>();
        }
        public virtual T? Get(object id)
        {
            return Ctx.Find<T>(id);
        }
        public virtual void Insert(T entity)
        {
            Ctx.Add(entity);
            Ctx.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            Ctx.Update(entity); 
            Ctx.SaveChanges();
        }
        public virtual void Delete(object id)
        {
            Ctx.Remove(id);
            Ctx.SaveChanges();
        }
        
    }
}
