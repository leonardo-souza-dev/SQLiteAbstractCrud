using System.Collections.Generic;
using System.Linq;

namespace SQLiteAbstractCrud
{
    internal class Fields
    {
        public List<Field> Itens { get; } = new();

        public void AdicionarCampo(string nome, string tipoCSharp, bool ehPrimaryKey)
        {
            Itens.Add(new Field(nome, tipoCSharp, ehPrimaryKey));
        }

        public string GetPrimaryKeyName()
        {
            return Itens.First(x => x.IsPrimaryKey).Name;
        }
        public string GetQuotePrimaryKey()
        {
            return Itens.First(x => x.IsPrimaryKey).Quote;
        }
    }
}