using System;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields03
{
    public class Test3Fields_StrPk_Int_Datetime_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

            new Test3Fields_StrPk_Int_Datetime_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Test3Fields_StrPk_Int_Datetime_Repository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string valorString = "fooValor";
            const int valorInt = 123;
            var valorDateTime = new DateTime(2000, 11, 14);
            var sut = new Test3Fields_StrPk_Int_Datetime_Repository(_pathFileDb);
            sut.Insert(new Test3Fields_StrPk_Int_Datetime(valorInt, valorDateTime, valorString));

            // act
            var result = sut.Get(valorString);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorString, result.Foo);
            Assert.AreEqual(valorInt, result.Bar);
            Assert.AreEqual(valorDateTime, result.Data);
        }

        [Test]
        public void MustGetByRange()
        {
            // arrange
            const string valorString1 = "fooValor1";
            const int valorInt1 = 123;
            var valorDateTime1 = new DateTime(2020, 3, 17);
            const string valorString2 = "fooValor2";
            const int valorInt2 = 456;
            var valorDateTime2 = new DateTime(2020, 4, 17);
            const string valorString3 = "fooValor3";
            const int valorInt3 = 789;
            var valorDateTime3 = new DateTime(2020, 5, 17);
            var sut = new Test3Fields_StrPk_Int_Datetime_Repository(_pathFileDb);
            sut.Insert(new Test3Fields_StrPk_Int_Datetime(valorInt1, valorDateTime1, valorString1));
            sut.Insert(new Test3Fields_StrPk_Int_Datetime(valorInt2, valorDateTime2, valorString2));
            sut.Insert(new Test3Fields_StrPk_Int_Datetime(valorInt3, valorDateTime3, valorString3));

            // act
            var result = sut.GetByDateRange("Data", new DateTime(2020, 3, 30), new DateTime(2021, 1, 1));

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string valorString = "fooValor";
            const int valorInt = 123;
            var valorDateTime = new DateTime(1983, 3, 14);
            var entidade = new Test3Fields_StrPk_Int_Datetime(valorInt, valorDateTime, valorString);
            var sut = new Test3Fields_StrPk_Int_Datetime_Repository(_pathFileDb);

            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorString);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorString, entidadeInserida.Foo);
            Assert.AreEqual(valorInt, entidadeInserida.Bar);
            Assert.AreEqual(valorDateTime, entidadeInserida.Data);
        }
    }

    public class Test3Fields_StrPk_Int_Datetime_Repository : RepositoryBase<Test3Fields_StrPk_Int_Datetime>
    {
        public Test3Fields_StrPk_Int_Datetime_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test3Fields_StrPk_Int_Datetime
    {
        public int Bar { get; init; }
        public DateTime Data { get; init; }
        [PrimaryKey]
        public string Foo { get; init; }

        public Test3Fields_StrPk_Int_Datetime(int bar, DateTime data, string foo)
        {
            Bar = bar;
            Data = data;
            Foo = foo;
        }
    }
}
