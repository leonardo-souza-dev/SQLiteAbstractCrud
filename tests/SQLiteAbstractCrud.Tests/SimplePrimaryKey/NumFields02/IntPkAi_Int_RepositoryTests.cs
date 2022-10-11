using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields02
{
    public class IntPkAi_Int_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new IntPkAi_Int_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new IntPkAi_Int_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            int id = 1;
            int intField = 123;
            var sut = new IntPkAi_Int_Repository(_pathFileDb);
            sut.Insert(new IntPkAi_Int(id, intField));

            // act
            var actual = sut.GetAll().First();

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(intField, actual.IntField);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const int valueMyProperty = 123;
            var entity = new IntPkAi_Int(1, valueMyProperty);
            var sut = new IntPkAi_Int_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.GetAll().First();
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(valueMyProperty, insertedEntity.IntField);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            const int valueMyProperty = 124;
            var entity = new IntPkAi_Int(1, valueMyProperty);
            var sut = new IntPkAi_Int_Repository(_pathFileDb);
            sut.Insert(entity);
            var newValueMyProperty = 445;

            // act
            var insertedEntity = sut.GetAll().First();
            insertedEntity.IntField = newValueMyProperty;
            _ = sut.Update(insertedEntity, nameof(insertedEntity.IntField), newValueMyProperty);

            // assert
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(newValueMyProperty, insertedEntity.IntField);
        }

        [Test]
        public void MustInsertBatch()
        {
            // arrange
            var valueMyProperty1 = 10;
            var valueMyProperty2 = 20;
            var valueMyProperty3 = 30;
            var entity1 = new IntPkAi_Int(1,valueMyProperty1);
            var entity2 = new IntPkAi_Int(2, valueMyProperty2);
            var entity3 = new IntPkAi_Int(3, valueMyProperty3);

            var sut = new IntPkAi_Int_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<IntPkAi_Int> { entity1, entity2, entity3 });

            // assert
            var insertedEntity1 = sut.GetAll().Where(x => x.IntField == valueMyProperty1);
            var insertedEntity2 = sut.GetAll().Where(x => x.IntField == valueMyProperty2);
            var insertedEntity3 = sut.GetAll().Where(x => x.IntField == valueMyProperty3);
            Assert.NotNull(insertedEntity1);
            Assert.NotNull(insertedEntity2);
            Assert.NotNull(insertedEntity3);
            Assert.AreEqual(valueMyProperty1, entity1.IntField);
            Assert.AreEqual(valueMyProperty2, entity2.IntField);
            Assert.AreEqual(valueMyProperty3, entity3.IntField);
        }
    }

    public class IntPkAi_Int_Repository : RepositoryBase<IntPkAi_Int>
    {
        public IntPkAi_Int_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class IntPkAi_Int
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; }

        public int IntField { get; set; }

        public IntPkAi_Int(int id, int intField)
        {
            Id = id;
            IntField = intField;
        }
    }
}
