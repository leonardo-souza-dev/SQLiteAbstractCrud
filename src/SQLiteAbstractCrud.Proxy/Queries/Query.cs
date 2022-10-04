using SQLiteAbstractCrud.Proxy;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public abstract class Query<T>
    {
        public readonly T _type;
        internal readonly ProxyBase _proxyBase;

        public string Raw => ToRaw(); 
        public string TableName => typeof(T).Name;

        public Query(T type)
        {
            this._type = type;
            this._proxyBase = new ProxyBase(typeof(T));
        }

        public abstract string ToRaw();
    }
}
