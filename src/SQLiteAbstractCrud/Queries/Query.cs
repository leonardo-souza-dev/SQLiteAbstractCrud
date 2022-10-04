using SQLiteAbstractCrud.Proxy;

namespace SQLiteAbstractCrud.Queries
{
    public abstract class Query<T>
    {
        public readonly T _type;
        public readonly Fields _fields;

        public string Raw => ToRaw(); 
        public string TableName => typeof(T).Name;

        public Query(T type)
        {
            this._type = type;
            this._fields = new ProxyBase(typeof(T)).Fields;
        }

        public abstract string ToRaw();
    }
}
