using System;

namespace SQLiteAbstractCrud.Proxy
{
    public class Field
    {
        public string NameOnDb { get; private set; }
        internal string TypeCSharp { get; set; }
        public string SQLiteType { get; private set; }
        public string Quote { get; private set; }
        public bool IsPrimaryKey { get; }
        public bool IsAutoincrement { get; }

        public Field(string name, string typeCSharp)
        {
            CtorConfig(name, typeCSharp);
        }
        
        public Field(string name, string typeCSharp, bool isPrimaryKey, bool isAutoincrement)
        {
            IsPrimaryKey = isPrimaryKey;
            IsAutoincrement = isAutoincrement;
            
            CtorConfig(name, typeCSharp);
        }

        private void CtorConfig(string nameOnDb, string typeCSharp)
        {
            NameOnDb = nameOnDb;
            TypeCSharp = typeCSharp;
            
            switch(typeCSharp)
            {
                case "String":
                case "DateTime":
                    SQLiteType = "TEXT";
                    Quote = "'";
                    break; 
                case "Int32": 
                case "Int16":
                case "Boolean":
                    SQLiteType = "INTEGER";
                    Quote = "";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCSharp), "TypeCSharp not found");
            }
        }
    }
}