using System;

namespace SQLiteAbstractCrud
{
    public class Field
    {
        public string NameOnDb { get; private set; }
        //public string Name { get; private set; }
        internal string TypeCSharp { get; set; }
        public string TypeSQLite { get; private set; }
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
                    TypeSQLite = "TEXT";
                    Quote = "'";
                    break; 
                case "Int32": 
                case "Int16":
                case "Boolean":
                    TypeSQLite = "INTEGER";
                    Quote = "";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeCSharp), "TypeCSharp not found");
            }
        }
    }
}