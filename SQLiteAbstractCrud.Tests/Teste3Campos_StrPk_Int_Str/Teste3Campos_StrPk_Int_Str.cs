namespace SQLiteAbstractCrud.Tests.Teste3Campos_StrPk_Int_Str
{
    public class Teste3Campos_StrPk_Int_Str
    {
        [PrimaryKey]
        public string Foo { get; init; }
        public int Bar { get; init; }
        public string Asdfg { get; init; }

        public Teste3Campos_StrPk_Int_Str(string foo, int bar, string asdfg)
        {
            Foo = foo;
            Bar = bar;
            Asdfg = asdfg;
        }
    }
}