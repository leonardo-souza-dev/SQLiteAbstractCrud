namespace SQLiteAbstractCrud.Tests.Teste1Campo_StrPk
{
    public class Teste1Campo_StrPk
    {
        [PrimaryKey]
        public string Foo { get; }

        public Teste1Campo_StrPk(string foo)
        {
            Foo = foo;
        }
    }
}