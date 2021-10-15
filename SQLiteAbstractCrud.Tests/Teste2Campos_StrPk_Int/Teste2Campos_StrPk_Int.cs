namespace SQLiteAbstractCrud.Tests.Teste2Campos_StrPk_Int
{
    public class Teste2Campos_StrPk_Int
    {
        [PrimaryKey]
        public string Foo { get; }
        public int Bar { get; }

        public Teste2Campos_StrPk_Int(string foo, int bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}