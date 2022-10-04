using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields02
{
    public class Test2Fields_IntPkAi_Int_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            var repo = new Test2Fields_IntPk_Int_Repository(_pathFileDb);
            repo.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Test2Fields_IntPk_Int_Repository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const int valueMyProperty = 123;
            var sut = new Test2Fields_IntPk_Int_Repository(_pathFileDb);
            sut.Insert(new Test2Fields_IntPkAi_Int(valueMyProperty));

            // act
            var result = sut.GetAll().ToList().First();

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valueMyProperty, result.MyProperty);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const int valueMyProperty = 123;
            var entity = new Test2Fields_IntPkAi_Int(valueMyProperty);
            var sut = new Test2Fields_IntPk_Int_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.GetAll().ToList().First();
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(valueMyProperty, insertedEntity.MyProperty);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            const int valueMyProperty = 124;
            var entity = new Test2Fields_IntPkAi_Int(valueMyProperty);
            var sut = new Test2Fields_IntPk_Int_Repository(_pathFileDb);
            sut.Insert(entity);
            var newValueMyProperty = 445;

            // act
            var insertedEntity = sut.GetAll().ToList().First();
            insertedEntity.SetMyProperty(newValueMyProperty);
            _ = sut.Update(insertedEntity, nameof(insertedEntity.MyProperty), newValueMyProperty);

            // assert
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(newValueMyProperty, insertedEntity.MyProperty);
        }

        [Test]
        public void MustInsertInBatch()
        {
            // arrange
            var valueMyProperty1 = 1;
            var valueMyProperty2 = 2;
            var valueMyProperty3 = 3;
            var entity1 = new Test2Fields_IntPkAi_Int(valueMyProperty1);
            var entity2 = new Test2Fields_IntPkAi_Int(valueMyProperty2);
            var entity3 = new Test2Fields_IntPkAi_Int(valueMyProperty3);

            var sut = new Test2Fields_IntPk_Int_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<Test2Fields_IntPkAi_Int> { entity1, entity2, entity3 });

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

    public class Test2Fields_IntPk_Int_Repository : RepositoryBase<Test2Fields_IntPkAi_Int>
    {
        public Test2Fields_IntPk_Int_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test2Fields_IntPkAi_Int
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; }
        public int MyProperty { get; private set; }

        public Test2Fields_IntPkAi_Int(int id, int myProperty)
        {
            Id = id;
            MyProperty = myProperty;
        }

        public Test2Fields_IntPkAi_Int(int myProperty)
        {
            MyProperty = myProperty;
        }

        public void SetMyProperty(int value)
        {
            MyProperty = value;
        }
    }
}
