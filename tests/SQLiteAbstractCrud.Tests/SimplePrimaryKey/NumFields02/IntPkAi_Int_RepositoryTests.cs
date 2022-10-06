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
            const int valueMyProperty = 123;
            var sut = new IntPkAi_Int_Repository(_pathFileDb);
            sut.Insert(new IntPkAi_Int(1, valueMyProperty));

            // act
            var result = sut.GetAll().First();

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valueMyProperty, result.MyProperty);
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
            Assert.AreEqual(valueMyProperty, insertedEntity.MyProperty);
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
            insertedEntity.MyProperty = newValueMyProperty;
            _ = sut.Update(insertedEntity, nameof(insertedEntity.MyProperty), newValueMyProperty);

            // assert
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(newValueMyProperty, insertedEntity.MyProperty);
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
            var insertedEntity1 = sut.GetAll().Where(x => x.MyProperty == valueMyProperty1);
            var insertedEntity2 = sut.GetAll().Where(x => x.MyProperty == valueMyProperty2);
            var insertedEntity3 = sut.GetAll().Where(x => x.MyProperty == valueMyProperty3);
            Assert.NotNull(insertedEntity1);
            Assert.NotNull(insertedEntity2);
            Assert.NotNull(insertedEntity3);
            Assert.AreEqual(valueMyProperty1, entity1.MyProperty);
            Assert.AreEqual(valueMyProperty2, entity2.MyProperty);
            Assert.AreEqual(valueMyProperty3, entity3.MyProperty);
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
        public int MyProperty { get; set; }

        public IntPkAi_Int(int id, int myProperty)
        {
            Id = id;
            MyProperty = myProperty;
        }
    }
}
