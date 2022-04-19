using System;

namespace SQLiteAbstractCrud.Tests.Teste3CamposStrPkStrPkStr
{
    public class Teste3Campos_StrPk_StrPk_Str
    {
        [PrimaryKey]
        public string Foo { get; init; }
        [PrimaryKey]
        public string Bar { get; init; }
        public string Zab { get; init; }

        public Teste3Campos_StrPk_StrPk_Str(string foo, string bar, string zab)
        {
            Foo = foo;
            Bar = bar;
            Zab = zab;
        }
    }
}