namespace SQLiteAbstractCrud.Tests.Teste1Campo_StrPk
{
    public class Teste1Campo_IntPkAi_StrPk
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Foo { get; }

        [PrimaryKey]
        public string OutroCampo { get; }

        public Teste1Campo_IntPkAi_StrPk(int foo, string outroCampo)
        {
            Foo = foo;
            OutroCampo = outroCampo;
        }
    }
}