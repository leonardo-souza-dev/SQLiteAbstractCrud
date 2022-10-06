using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SQLiteAbstractCrud.Proxy
{
    public class ProxyBase
    {
        private Type _type;
        private readonly List<ProxyPropertyInfo> _proxyPropertiesInfos = new();
        private readonly List<ProxyCtorParameterInfo> _proxyCtorParametersInfos = new();

        internal Fields Fields;

        public ProxyBase(Type type)
        {
            this._type = type;
            
            var ctorParameters = _type.GetConstructors().Single().GetParameters().ToList();
            ctorParameters.ForEach(p => _proxyCtorParametersInfos.Add(new ProxyCtorParameterInfo(p)));

            var propertiesInfos = this._type.GetProperties().ToList();
            propertiesInfos.ForEach(p => _proxyPropertiesInfos.Add(new ProxyPropertyInfo(p)));

            Fields = new Fields(_proxyPropertiesInfos);

            //ctorParameters.ForEach(ctorParameter =>Fields.AddField(new Field(ctorParameter)));
            //foreach (var item in _proxyPropertiesInfos)
            //{
            //    Fields.AddField(item.OriginalName, item.CSharpType, item.IsPrimaryKey, item.IsAutoIncrement);
            //}


            //var originalProperties = this._type.GetProperties().ToList();

            //int i = 0;
            //originalProperties.ForEach(x =>
            //{
            //    _originalPropertiesInfos.Add(new OriginalPropertyInfo(i, x.Name));
            //    i++;
            //});

            //int h = 0;
            //originalProperties.ForEach(originalProperty =>
            //{
            //    var originalOrder = _originalPropertiesInfos.First(y => y.Name == originalProperty.Name).Order;
            //    _proxyPropertiesInfos.Add(new ProxyPropertyInfo(originalOrder, originalProperty, h));
            //    h++;
            //});
            //foreach (var item in _proxyPropertiesInfos)
            //{
            //    Fields.AddField(item.OriginalName, item.CSharpType, item.IsPrimaryKey, item.IsAutoIncrement);
            //}
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
        public int GetCtorParametersCount()
        {
            return this._proxyCtorParametersInfos.Count;
        }

        public string GetFieldTypeSQLite(int index)
        {
            return this.Fields.Items[index].SQLiteType;
        }

        public string GetCtorParameterSQLiteType(int index)
        {
            return this._proxyCtorParametersInfos[index].SQLiteType;
        }

        public string GetFieldTypeCSharp(int index)
        {
            return this.Fields.Items[index].TypeCSharp;
        }

        public List<ProxyCtorParameterInfo> GetCtorParemetersInfos()
        {
            return this._proxyCtorParametersInfos;
        }

        public string GetCtorParemeterCSharpType(int index)
        {
            return this._proxyCtorParametersInfos[index].CSharpType;
        }

        public string GetTableName()
        {
            return _type.Name;
        }

        public bool FieldExists(string fieldName)
        {
            return Fields.Items.Any(x => x.NameOnDb.ToLower() == fieldName.ToLower());
        }
    }
}
