using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
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
            Assert.Throws<ApplicationException>(() => new Test2Fields_IntPkAi_StrPk_Repository(_pathFileDb));
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
        public int Field1 { get; }

        [PrimaryKey]
        public string Field2 { get; }

        public Test2Fields_IntPkAi_StrPk(int field1, string field2)
        {
            Field1 = field1;
            Field2 = field2;
        }
    }
}
