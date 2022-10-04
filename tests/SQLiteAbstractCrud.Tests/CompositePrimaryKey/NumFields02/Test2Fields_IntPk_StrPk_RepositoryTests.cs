using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;
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

        [Test]
        public void MustGet()
        {
            // arrange
            var intField = 1;
            var stringFiled = "10";
            var sut = new Test2Fields_IntPk_StrPk_Repository(_pathFileDb);
            sut.InsertBatch(new List<Test2Fields_IntPk_StrPk>
            {
                new Test2Fields_IntPk_StrPk(intField, stringFiled),
                new Test2Fields_IntPk_StrPk(2, "20"),
            });

            // act
            var actual = sut.Get(1, 10);

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(intField, actual.IntField);
            Assert.AreEqual(stringFiled, actual.StringField);
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
