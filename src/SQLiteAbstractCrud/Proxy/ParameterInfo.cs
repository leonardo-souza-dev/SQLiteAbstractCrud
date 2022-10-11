using System;
using System.Reflection;

namespace SQLiteAbstractCrud.Proxy
{
    public sealed class ProxyCtorParameterInfo
    {
        public string Name { get; set; }
        public string SQLiteType { get; set; }
        public string CSharpType { get; set; }

        public static string GetSQLiteType(string csharpType)
        {
            switch (csharpType)
            {
                case "String":
                case "DateTime":
                    return "TEXT";
                case "Int32":
                case "Int16":
                case "Boolean":
                    return "INTEGER";
                default:
                    throw new ArgumentOutOfRangeException(nameof(csharpType), "CsharpType not found");
            }
        }

        public ProxyCtorParameterInfo(ParameterInfo parameterInfo)
        {
            this.Name = parameterInfo.Name;
            this.CSharpType = parameterInfo.ParameterType.Name;
            this.SQLiteType = GetSQLiteType(this.CSharpType);
        }
    }
}
