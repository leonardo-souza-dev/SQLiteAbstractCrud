using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields01
{
    public class StrPk_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new StrPk_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new StrPk_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string stringFieldValue = "value";
            var sut = new StrPk_Repository(_pathFileDb);
            sut.Insert(new StrPk(stringFieldValue));

            // act
            var actual = sut.Get(stringFieldValue);

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(stringFieldValue, actual.StringField);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string field1 = "fooValue";
            var entity = new StrPk(field1);
            var sut = new StrPk_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var inserteEntity = sut.Get(field1);
            Assert.NotNull(inserteEntity);
            Assert.AreEqual(field1, inserteEntity.StringField);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            var stringField = "valueString";
            var entity = new StrPk(stringField);
            var sut = new StrPk_Repository(_pathFileDb);
            sut.Insert(entity);
            var newValueStringField = "newValue";

            // act
            _ = sut.Update(entity, nameof(entity.StringField), newValueStringField);

            // assert
            var insertedEntityWithOldValue = sut.Get(stringField);
            Assert.Null(insertedEntityWithOldValue);
            var insertedEntityWithUpdatedValue = sut.GetAll().First(x => x.StringField == newValueStringField);
            Assert.NotNull(insertedEntityWithUpdatedValue);
            Assert.AreEqual(newValueStringField, insertedEntityWithUpdatedValue.StringField);
        }

        [Test]
        public void MustGetAll()
        {
            // arrange
            var entity1 = new StrPk("fooValue1");
            var entity2 = new StrPk("fooValue2");
            var sut = new StrPk_Repository(_pathFileDb);
            sut.Insert(entity1);
            sut.Insert(entity2);

            // act
            var actual = sut.GetAll();

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(2, actual.Count());
            Assert.True(actual.FirstOrDefault(x => x.StringField == entity1.StringField) != null);
            Assert.True(actual.FirstOrDefault(x => x.StringField == entity2.StringField) != null);
        }

        [Test]
        public void MustInsertInBatch()
        {
            // arrange
            var value1 = "fooValue1";
            var value2 = "fooValue2";
            var value3 = "fooValue3";
            var entity1 = new StrPk(value1);
            var entity2 = new StrPk(value2);
            var entity3 = new StrPk(value3);

            var sut = new StrPk_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<StrPk> { entity1, entity2, entity3 });

            // assert
            var insertedEntity1 = sut.Get(value1);
            var insertedEntity2 = sut.Get(value2);
            var insertedEntity3 = sut.Get(value3);
            Assert.NotNull(insertedEntity1);
            Assert.NotNull(insertedEntity2);
            Assert.NotNull(insertedEntity3);
            Assert.AreEqual(value1, entity1.StringField);
            Assert.AreEqual(value2, entity2.StringField);
            Assert.AreEqual(value3, entity3.StringField);
        }
    }

    public class StrPk_Repository : RepositoryBase<StrPk>
    {
        public StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class StrPk
    {
        [PrimaryKey]
        public string StringField { get; }

        public StrPk(string stringField)
        {
            StringField = stringField;
        }
    }
}
