using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Models.Base;

namespace rest_with_asp_net_10_cviana.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T entity);
        T Update(T entity);
        void Delete(long id);
        List<T> FindAll();
        T FindById(long id);
        bool Exists(long id);
        List<T> FindWithPagedSearch(string query);
        int GetCount(string query);
    }
}
