using System;

namespace SQLiteAbstractCrud.Tests.Teste2Campos_StrPk_Int
{
    public class Teste2Campos_DateTimePk_Bool
    {
        [PrimaryKey]
        public DateTime CampoDateTime { get; }
        public bool CampoBool { get; }

        public Teste2Campos_DateTimePk_Bool(DateTime foo, bool bar)
        {
            CampoDateTime = foo;
            CampoBool = bar;
        }
    }
}