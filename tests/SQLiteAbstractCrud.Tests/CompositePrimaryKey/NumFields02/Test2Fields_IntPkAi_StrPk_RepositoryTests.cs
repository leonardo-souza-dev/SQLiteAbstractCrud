using System;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields02
{
    public class Test2Fields_IntPkAi_StrPk_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
        }

        [Test]
        public void WhenEntityHasOneFieldIntPkAiAndCompositePk_MustThrowException()
        {
            // arrange, act & assert
            Assert.Throws<AggregateException>(() => new Test2Fields_IntPkAi_StrPk_Repository(_pathFileDb));
        }
    }

    public class Test2Fields_IntPkAi_StrPk_Repository : RepositoryBase<Test2Fields_IntPkAi_StrPk>
    {
        public Test2Fields_IntPkAi_StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test2Fields_IntPkAi_StrPk
    {
        [PrimaryKey]
        [AutoIncrement]
        public int IntField { get; }

        [PrimaryKey]
        public string StringField { get; }

        public Test2Fields_IntPkAi_StrPk(int intField, string stringField)
        {
            IntField = intField;
            StringField = stringField;
        }
    }
}
