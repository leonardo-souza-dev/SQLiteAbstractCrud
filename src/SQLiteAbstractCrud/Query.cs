using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace SQLiteAbstractCrud
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
