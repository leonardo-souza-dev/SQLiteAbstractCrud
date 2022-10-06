using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields03
{
    public class Test3Fields_StrPk_Int_Str_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void Setup()
        {
            new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string fooValue = "fooValue";
            const int barValue = 123;
            const string asdfgValue = "qwerty";
            var sut = new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb);
            sut.Insert(new Test3Fields_StrPk_Int_Str(fooValue, barValue, asdfgValue));

            // act
            var result = sut.Get(fooValue);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(fooValue, result.Abc);
            Assert.AreEqual(barValue, result.Bar);
            Assert.AreEqual(asdfgValue, result.Cde);
        }

        //[Test]
        //public void MustInsert()
        //{
        //    // arrange
        //    const string stringPkValue = "fooPkValue";
        //    const int intValue = 123;
        //    const string stringValue = "qwerty";
        //    var entity = new Test3Fields_StrPk_Int_Str(intValue, stringPkValue, stringValue);
        //    var sut = new StrPk_Int_Str_Repository(_pathFileDb);

        //    // act
        //    sut.Insert(entity);

        //    // assert
        //    var insertedEntity = sut.Get(stringPkValue);
        //    Assert.NotNull(insertedEntity);
        //    Assert.AreEqual(stringPkValue, insertedEntity.Abc);
        //    Assert.AreEqual(intValue, insertedEntity.Bar);
        //    Assert.AreEqual(stringValue, insertedEntity.Cde);
        //}

        //[Test]
        //public void MustInsertInBatch()
        //{
        //    // arrange
        //    var stringPkValue1 = "fooPkValue";
        //    var intValue1 = 1;
        //    var stringValue1 = "fooValue";

        //    var stringPkValue2 = "fooPkValue2";
        //    var intValue2 = 2;
        //    var stringValue2 = "fooValue2";

        //    var stringPkValue3 = "fooPkValue3";
        //    var intValue3 = 3;
        //    var stringValue3 = "fooValue3";

        //    var entity1 = new Test3Fields_StrPk_Int_Str(intValue1, stringPkValue1, stringValue1);
        //    var entity2 = new Test3Fields_StrPk_Int_Str(intValue2, stringPkValue2, stringValue2);
        //    var entity3 = new Test3Fields_StrPk_Int_Str(intValue3, stringPkValue3, stringValue3);

        //    var sut = new StrPk_Int_Str_Repository(_pathFileDb);

        //    // act
        //    sut.InsertBatch(new List<Test3Fields_StrPk_Int_Str> { entity1, entity2, entity3 });

        //    // assert
        //    var insertedEntity1 = sut.Get(stringPkValue1);
        //    var insertedEntity2 = sut.Get(stringPkValue2);
        //    var insertedEntity3 = sut.Get(stringPkValue3);
        //    Assert.NotNull(insertedEntity1);
        //    Assert.NotNull(insertedEntity2);
        //    Assert.NotNull(insertedEntity3);
        //    Assert.AreEqual(stringPkValue1, entity1.Abc);
        //    Assert.AreEqual(stringPkValue2, entity2.Abc);
        //    Assert.AreEqual(stringPkValue3, entity3.Abc);
        //}

        //[Test]
        //public void MustUpdate()
        //{
        //    // arrange
        //    const string stringValue = "valorString";
        //    const int intValue = 123;
        //    const string stringValue2 = "valorString2";
        //    const string stringValueNew = "valorString2novo";
        //    var entity = new Test3Fields_StrPk_Int_Str(intValue, stringValue, stringValue2);
        //    var sut = new StrPk_Int_Str_Repository(_pathFileDb);
        //    sut.Insert(entity);

        //    // act
        //    _ = sut.Update(entity, "Cde", stringValueNew);

        //    // assert
        //    var updatedEntity = sut.Get(stringValue);
        //    Assert.NotNull(updatedEntity);
        //    Assert.AreEqual(stringValue, updatedEntity.Abc);
        //    Assert.AreEqual(intValue, updatedEntity.Bar);
        //    Assert.AreEqual(stringValueNew, updatedEntity.Cde);
        //}
    }

    public class Test3Fields_StrPk_Int_Str_Repository : RepositoryBase<Test3Fields_StrPk_Int_Str>
    {
        public Test3Fields_StrPk_Int_Str_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test3Fields_StrPk_Int_Str
    {
        [PrimaryKey]
        public string Abc { get; init; }
        public int Bar { get; init; }
        public string Cde { get; init; }

        public Test3Fields_StrPk_Int_Str(string abc, int bar, string cde)
        {
            Abc = abc;
            Bar = bar;
            Cde = cde;
        }
    }
}
