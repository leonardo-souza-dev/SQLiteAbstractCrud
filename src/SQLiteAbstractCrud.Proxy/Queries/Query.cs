using SQLiteAbstractCrud.Proxy;
using System.Reflection;
using System.Xml.Linq;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public abstract class Query<T>
    {
        private readonly T _type;
        internal readonly ProxyBase _proxyBase;

        public string Raw => ToRaw(); 
        public string TableName => typeof(T).Name;

        public Query(T type)
        {
            this._type = type;
            this._proxyBase = new ProxyBase(typeof(T));
        }

        public abstract string ToRaw();

        public PropertyInfo GetPropertyInfo(string property)
        {
            return _type.GetType().GetProperty(property);
        }

        public object GetRawValue(string fieldName)
        {
            return _type.GetType().GetProperty(fieldName).GetValue(_type, null);
        }

        public object GetPkValue(string fieldName)
        {
            return _type.GetType().GetProperty(fieldName).GetValue(_type, null);
        }
    }
}
