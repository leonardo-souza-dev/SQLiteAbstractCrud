using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields02
{
    public class IntPk_StrPk_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new IntPk_StrPk_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new IntPk_StrPk_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustDelete()
        {
            // arrange
            const int intValue = 123;
            const string stringValue = "123";
            var sut = new IntPk_StrPk_Repository(_pathFileDb);
            sut.Insert(new IntPk_StrPk(intValue, stringValue));

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
            var sut = new IntPk_StrPk_Repository(_pathFileDb);
            sut.InsertBatch(new List<IntPk_StrPk>
            {
                new IntPk_StrPk(intField, stringFiled),
                new IntPk_StrPk(2, "20"),
            });

            // act
            var actual = sut.Get(1, 10);

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(intField, actual.IntField);
            Assert.AreEqual(stringFiled, actual.StringField);
        }
    }

    public class IntPk_StrPk_Repository : RepositoryBase<IntPk_StrPk>
    {
        public IntPk_StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class IntPk_StrPk
    {
        [PrimaryKey]
        public int IntField { get; }

        [PrimaryKey]
        public string StringField { get; }

        public IntPk_StrPk(int intField, string stringField)
        {
            IntField = intField;
            StringField = stringField;
        }
    }
}
