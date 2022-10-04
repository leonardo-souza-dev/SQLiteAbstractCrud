using SQLiteAbstractCrud.Proxy;

namespace SQLiteAbstractCrud.Queries
{
    public abstract class Query<T>
    {
        public readonly T _type;
        public readonly ProxyBase _proxy;

        public string Raw => ToRaw(); 
        public string TableName => typeof(T).Name;

        public Query(T type)
        {
            this._type = type;
            this._proxy = new ProxyBase(typeof(T));
        }

        public abstract string ToRaw();
    }
}
