using System.Collections.Generic;

namespace SQLiteAbstractCrud
{
    public interface IRepository<T>
    {
        T Insert(T t);
        void InsertBatch(List<T> list);
        void Update(T t);
        IEnumerable<T> GetAll();
        T Get(object id);
        void Delete(object id);
        void DropTable();
    }
}