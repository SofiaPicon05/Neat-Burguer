using Neat_Burguer.Models.Entities;

namespace Neat_Burguer.Repositories
{
    public class Repository <T> where T : class
    {
      
        public NeatContext Context { get; }
        public Repository(NeatContext context)
        {
            Context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }
        public virtual T? Get(string id)
        {
            return Context.Find<T>(id);
        }
        public T? GetById(int id)
        {
            return Context.Find<T>(id);
        }
        public void Insert(T entity)
        {
            Context.Add(entity);
            Context.SaveChanges();
        }
        public void Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
        }
        public void Delete(T entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }
        public void DeleteAll(string id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }
    }
}
