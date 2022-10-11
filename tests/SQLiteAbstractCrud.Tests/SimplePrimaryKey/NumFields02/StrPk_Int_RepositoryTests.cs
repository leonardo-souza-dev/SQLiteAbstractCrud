using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields02
{
    public class StrPk_Int_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new StrPk_Int_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new StrPk_Int_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string stringValue = "stringValue";
            const int intValue = 123;
            var sut = new StrPk_Int_Repository(_pathFileDb);
            sut.Insert(new StrPk_Int(stringValue, intValue));

            // act
            var actual = sut.Get(stringValue);

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(stringValue, actual.StringField);
            Assert.AreEqual(intValue, actual.IntField);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string stringValue = "stringValue";
            const int intValue = 123;
            var entity = new StrPk_Int(stringValue, intValue);
            var sut = new StrPk_Int_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.Get(stringValue);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(stringValue, insertedEntity.StringField);
            Assert.AreEqual(intValue, insertedEntity.IntField);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            const string stringValue = "stringValue";
            const int intValue = 124;
            var entity = new StrPk_Int(stringValue, intValue);
            var sut = new StrPk_Int_Repository(_pathFileDb);
            sut.Insert(entity);
            var newValueInt = 445;

            // act
            _ = sut.Update(entity, "IntField", newValueInt);

            // assert
            var insertedEntity = sut.Get(stringValue);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(stringValue, insertedEntity.StringField);
            Assert.AreEqual(newValueInt, insertedEntity.IntField);
        }

        [Test]
        public void MustInsertInBatch()
        {
            // arrange
            var stringField1 = "stringField1";
            var stringField2 = "stringField2";
            var stringField3 = "stringField3";
            var intField1 = 1;
            var intField2 = 2;
            var intField3 = 3;
            var entity1 = new StrPk_Int(stringField1, intField1);
            var entity2 = new StrPk_Int(stringField2, intField2);
            var entity3 = new StrPk_Int(stringField3, intField3);

            var sut = new StrPk_Int_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<StrPk_Int> { entity1, entity2, entity3 });

            // assert
            var insertedEntity1 = sut.Get(stringField1);
            var insertedEntity2 = sut.Get(stringField2);
            var insertedEntity3 = sut.Get(stringField3);
            Assert.NotNull(insertedEntity1);
            Assert.NotNull(insertedEntity2);
            Assert.NotNull(insertedEntity3);
            Assert.AreEqual(stringField1, entity1.StringField);
            Assert.AreEqual(stringField2, entity2.StringField);
            Assert.AreEqual(stringField3, entity3.StringField);
        }
    }

    public class StrPk_Int_Repository : RepositoryBase<StrPk_Int>
    {
        public StrPk_Int_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class StrPk_Int
    {
        public int IntField { get; }
        [PrimaryKey]
        public string StringField { get; }

        public StrPk_Int(string stringField, int intField)
        {
            StringField = stringField;
            IntField = intField;
        }
    }
}
