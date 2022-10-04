using System.Collections.Generic;
using System.Linq;

namespace SQLiteAbstractCrud
{
    public class Fields
    {
        public List<Field> Items { get; } = new();

        public void AddField(string name, string csharpType, bool isPrimaryKey, bool isAutoincrement)
        {
            Items.Add(new Field(name, csharpType, isPrimaryKey, isAutoincrement));
        }

        public string GetPrimaryKeyName()
        {
            return Items.First(x => x.IsPrimaryKey).NameOnDb;
        }

        public string GetQuotePrimaryKey()
        {
            return Items.First(x => x.IsPrimaryKey).Quote;
        }

        public string GetPrimaryKeyType()
        {
            return Items.First(x => x.IsPrimaryKey).TypeCSharp;
        }
    }
}