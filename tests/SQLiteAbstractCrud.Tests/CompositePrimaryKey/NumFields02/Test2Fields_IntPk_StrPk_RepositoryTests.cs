using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields03;

namespace SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields02
{
    public class Test2Fields_IntPk_StrPk_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
        }

        [Test]
        public void MustDelete()
        {
            // arrange
            const int intValue = 123;
            const string stringValue = "123";
            var sut = new Test2Fields_IntPk_StrPk_Repository(_pathFileDb);
            sut.Insert(new Test2Fields_IntPk_StrPk(intValue, stringValue));

            // act
            sut.Delete(intValue, stringValue);

            // assert
            var all = sut.GetAll();
            Assert.False(all.Any(x => x.IntField == intValue && x.StringField == stringValue));
        }
    }

    public class Test2Fields_IntPk_StrPk_Repository : RepositoryBase<Test2Fields_IntPk_StrPk>
    {
        public Test2Fields_IntPk_StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test2Fields_IntPk_StrPk
    {
        [PrimaryKey]
        public int IntField { get; }

        [PrimaryKey]
        public string StringField { get; }

        public Test2Fields_IntPk_StrPk(int intField, string stringField)
        {
            IntField = intField;
            StringField = stringField;
        }
    }
}
