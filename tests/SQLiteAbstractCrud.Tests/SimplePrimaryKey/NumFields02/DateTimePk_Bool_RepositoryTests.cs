using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields02
{
    public class DateTimePk_Bool_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {            
            new DateTimePk_Bool_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new DateTimePk_Bool_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            var dateTimeField = DateTime.Now;
            var boolField = true;
            var entity = new DateTimePk_Bool(boolField, dateTimeField);
            var sut = new DateTimePk_Bool_Repository(_pathFileDb);
            sut.Insert(entity);
            var newValueBoolField = false;

            // act
            _ = sut.Update(entity, nameof(entity.BoolField), newValueBoolField);

            // assert
            var insertedEntity = sut.Get(dateTimeField);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(dateTimeField.Year, insertedEntity.DateTimeField.Year);
            Assert.AreEqual(dateTimeField.Month, insertedEntity.DateTimeField.Month);
            Assert.AreEqual(dateTimeField.Day, insertedEntity.DateTimeField.Day);
            Assert.AreEqual(dateTimeField.Hour, insertedEntity.DateTimeField.Hour);
            Assert.AreEqual(dateTimeField.Minute, insertedEntity.DateTimeField.Minute);
            Assert.AreEqual(dateTimeField.Second, insertedEntity.DateTimeField.Second);
            Assert.AreEqual(dateTimeField.Millisecond, insertedEntity.DateTimeField.Millisecond);
            Assert.AreEqual(newValueBoolField, insertedEntity.BoolField);
        }

        [Test]
        public void MustGet()
        {
            // arrange
            var entity1 = new DateTimePk_Bool(true, new DateTime(2000, 1, 10));
            var entity2 = new DateTimePk_Bool(false, new DateTime(2000, 1, 20));
            var sut = new DateTimePk_Bool_Repository(_pathFileDb);
            sut.InsertBatch(new List<DateTimePk_Bool> { entity1, entity2 });

            // act
            var actual = sut.GetAll();

            // assert
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(10, actual.FirstOrDefault(x => x.BoolField).DateTimeField.Day);
            Assert.AreEqual(20, actual.FirstOrDefault(x => !x.BoolField).DateTimeField.Day);
        }

        [Test]
        public void GivenAnDbWithOneItem_WhenDeleteThisOne_ThenDbIsEmpty()
        {
            // arrange
            var pk = new DateTime(2000, 12, 10);
            var sut = new DateTimePk_Bool_Repository(_pathFileDb);
            sut.Insert(new DateTimePk_Bool(true, pk));

            // act
            sut.Delete(pk);

            // assert
            var allItems = sut.GetAll();
            Assert.NotNull(allItems);
            Assert.AreEqual(0, allItems.Count());
        }

        [Test]
        public void GivenAnEmptyDb_WhenGetAll_ThenMustGetNothing()
        {
            // arrange
            var sut = new DateTimePk_Bool_Repository(_pathFileDb);

            // act
            var actual = sut.GetAll();

            // assert
            Assert.AreEqual(0, actual.Count());
        }
    }

    public class DateTimePk_Bool_Repository : RepositoryBase<DateTimePk_Bool>
    {
        public DateTimePk_Bool_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class DateTimePk_Bool
    {
        public bool BoolField { get; }

        [PrimaryKey]
        public DateTime DateTimeField { get; }

        public DateTimePk_Bool(bool boolField, DateTime dateTimeField)
        {
            DateTimeField = dateTimeField;
            BoolField = boolField;
        }
    }
}
