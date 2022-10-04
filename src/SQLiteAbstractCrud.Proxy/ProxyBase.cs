using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQLiteAbstractCrud.Proxy
{
    public class ProxyBase
    {
        private Type _type;
        private readonly List<OriginalPropertyInfo> _originalPropertiesInfos = new();
        private readonly List<ProxyPropertyInfo> _proxyPropertiesInfos = new();

        internal Fields Fields { get; } = new();

        public ProxyBase(Type type)
        {
            this._type = type;
            var properties = this._type.GetProperties().ToList();

            int i = 0;
            properties.ForEach(x =>
            {
                _originalPropertiesInfos.Add(new OriginalPropertyInfo(i, x.Name));
                i++;
            });

            int j = 0;
            properties.OrderBy(x => x.Name).ToList().ForEach(x =>
            {
                var originalOrder = _originalPropertiesInfos.First(y => y.Name == x.Name).Order;
                _proxyPropertiesInfos.Add(new ProxyPropertyInfo(originalOrder, x, j));
                j++;
            });

            foreach (var item in _proxyPropertiesInfos)
            {
                Fields.AddField(item.OriginalName, item.CSharpType, item.IsPrimaryKey, item.IsAutoIncrement);
            }
        }

        [Obsolete]
        public string GetPrimaryKeyName()
        {
            return Fields.GetPrimaryKeyName();
        }

        [Obsolete]
        public string GetQuotePrimaryKey()
        {
            return Fields.GetQuotePrimaryKey();
        }

        public List<string> GetFieldsNames()
        {
            return Fields.Items.Select(x => x.NameOnDb).ToList();
        }

        public List<Field> GetFieldsNotAutoIncrement()
        {
            return Fields.Items.Where(x => !x.IsAutoincrement).ToList();
        }

        public int GetFieldsCount()
        {
            return this.Fields.Items.Count;
        }

        public string GetFieldTypeSQLite(int index)
        {
            return this.Fields.Items[index].TypeSQLite;
        }

        public string GetFieldTypeCSharp(int index)
        {
            return this.Fields.Items[index].TypeCSharp;
        }
    }
}
