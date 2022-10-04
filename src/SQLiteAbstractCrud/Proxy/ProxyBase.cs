using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQLiteAbstractCrud.Proxy
{
    public class ProxyBase
    {
        private Type _type;
        private readonly List<OriginalPropertyInfo> _originalPropertiesInfos = new ();
        private readonly List<ProxyPropertyInfo> _proxyPropertiesInfos = new ();

        public Fields Fields { get; } = new ();

        public ProxyBase(Type type)
        {
            this._type = type;
            
            int i = 0;
            this._type.GetProperties().ToList().ForEach(x =>
            {
                _originalPropertiesInfos.Add(new OriginalPropertyInfo(i, x.Name));
                i++;
            });

            int j = 0;
            this._type.GetProperties().OrderBy(x => x.Name).ToList().ForEach(x =>
            {
                var originalOrder = _originalPropertiesInfos.First(y => y.Name == x.Name).Order;
                _proxyPropertiesInfos.Add(new ProxyPropertyInfo(originalOrder, x, 0));
                j++;
            });

            foreach (var item in _proxyPropertiesInfos)
            {
                Fields.AddField(item.OriginalName, item.CSharpType, item.IsPrimaryKey, item.IsAutoIncrement);
            }
        }
    }
}
