using System;

namespace SQLiteAbstractCrud.Tests.Teste3CamposStrPkStrPkStr
{
    public class Teste3CamposStrPkStrPkStr
    {
        [PrimaryKey]
        public string Foo { get; init; }
        [PrimaryKey]
        public string Bar { get; init; }
        public string Zab { get; init; }

        public Teste3CamposStrPkStrPkStr(string foo, string bar, string zab)
        {
            Foo = foo;
            Bar = bar;
            Zab = zab;
        }
    }
}