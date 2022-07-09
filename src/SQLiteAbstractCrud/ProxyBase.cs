using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQLiteAbstractCrud
{
    internal class ProxyBase
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

    public sealed class ProxyPropertyInfo
    {
        public int OriginalOrder { get; }
        private PropertyInfo _propertyInfo;
        public string OriginalName { get { return _propertyInfo.Name; } }
        public string CSharpType { get { return _propertyInfo.PropertyType.Name; } }
        public bool IsPrimaryKey
        {
            get
            {
                var primaryKeyAttribute = _propertyInfo.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                return primaryKeyAttribute.Any();
            }
        }
        public bool IsAutoIncrement
        {
            get
            {
                var autoIncrementAttribute = _propertyInfo.GetCustomAttributes(typeof(AutoIncrementAttribute), true);
                return autoIncrementAttribute.Any();
            }
        }
        public int ProxyOrder { get; }

        public ProxyPropertyInfo(int originalOrder, PropertyInfo propertyInfo, int proxyOrder)
        {
            this.OriginalOrder = originalOrder;
            this._propertyInfo = propertyInfo;
            this.ProxyOrder = proxyOrder;
        }
    }

    public sealed class OriginalPropertyInfo
    {
        public int Order { get; }
        public string Name { get; }

        public OriginalPropertyInfo(int order, string name)
        {
            this.Order = order;
            this.Name = name;
        }
    }
}
