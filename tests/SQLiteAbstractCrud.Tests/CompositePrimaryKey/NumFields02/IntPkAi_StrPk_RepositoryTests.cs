using System;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields02
{
    public class IntPkAi_StrPk_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [Test]
        public void WhenEntityHasOneFieldIntPkAiAndCompositePk_MustThrowException()
        {
            // arrange, act & assert
            Assert.Throws<AggregateException>(() => new IntPkAi_StrPk_Repository(_pathFileDb));
        }
    }

    public class IntPkAi_StrPk_Repository : RepositoryBase<IntPkAi_StrPk>
    {
        public IntPkAi_StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class IntPkAi_StrPk
    {
        [PrimaryKey]
        [AutoIncrement]
        public int IntField { get; }

        [PrimaryKey]
        public string StringField { get; }

        public IntPkAi_StrPk(int intField, string stringField)
        {
            IntField = intField;
            StringField = stringField;
        }
    }
}
