using System.Linq;
using System.Reflection;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Proxy
{
    public sealed class ProxyPropertyInfo
    {
        public int OriginalOrder { get; }
        private PropertyInfo _propertyInfo;
        public string OriginalName => _propertyInfo.Name;

        public string CSharpType => _propertyInfo.PropertyType.Name;

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

        //public ProxyPropertyInfo(int originalOrder, PropertyInfo propertyInfo, int proxyOrder)
        //{
        //    this.OriginalOrder = originalOrder;
        //    this._propertyInfo = propertyInfo;
        //    this.ProxyOrder = proxyOrder;
        //}

        public ProxyPropertyInfo(PropertyInfo propertyInfo)
        {
            this._propertyInfo = propertyInfo;
        }
    }    
}
