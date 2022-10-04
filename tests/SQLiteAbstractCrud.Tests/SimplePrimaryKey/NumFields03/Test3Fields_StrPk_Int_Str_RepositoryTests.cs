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
            var repo = new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb);
            repo.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb);
            repo.DropTable();
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

        [Test]
        public void MustInsert()
        {
            // arrange
            const string fooValue = "fooValue";
            const int barValue = 123;
            const string asdfgValue = "qwerty";
            var entity = new Test3Fields_StrPk_Int_Str(fooValue, barValue, asdfgValue);
            var sut = new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.Get(fooValue);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(fooValue, insertedEntity.Abc);
            Assert.AreEqual(barValue, insertedEntity.Bar);
            Assert.AreEqual(asdfgValue, insertedEntity.Cde);
        }

        [Test]
        public void MustInsertInBatch()
        {
            // arrange
            var valorStrPk1 = "fooValor1";
            var valorStrPk2 = "fooValor2";
            var valorStrPk3 = "fooValor3";
            var valorInt1 = 1;
            var valorInt2 = 2;
            var valorInt3 = 3;
            var valorStr1 = "fooValor1";
            var valorStr2 = "fooValor2";
            var valorStr3 = "fooValor3";
            var entidade1 = new Test3Fields_StrPk_Int_Str(valorStrPk1, valorInt1, valorStr1);
            var entidade2 = new Test3Fields_StrPk_Int_Str(valorStrPk2, valorInt2, valorStr2);
            var entidade3 = new Test3Fields_StrPk_Int_Str(valorStrPk3, valorInt3, valorStr3);

            var sut = new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<Test3Fields_StrPk_Int_Str> { entidade1, entidade2, entidade3 });

            // assert
            var entidadeInserida1 = sut.Get(valorStrPk1);
            var entidadeInserida2 = sut.Get(valorStrPk2);
            var entidadeInserida3 = sut.Get(valorStrPk3);
            Assert.NotNull(entidadeInserida1);
            Assert.NotNull(entidadeInserida2);
            Assert.NotNull(entidadeInserida3);
            Assert.AreEqual(valorStrPk1, entidade1.Abc);
            Assert.AreEqual(valorStrPk2, entidade2.Abc);
            Assert.AreEqual(valorStrPk3, entidade3.Abc);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            const string valorString = "valorString";
            const int valorInt = 123;
            const string valorString2 = "valorString2";
            const string valorString2Novo = "valorString2novo";
            var entidade = new Test3Fields_StrPk_Int_Str(valorString, valorInt, valorString2);
            var sut = new Test3Fields_StrPk_Int_Str_Repository(_pathFileDb);
            sut.Insert(entidade);

            // act
            _ = sut.Update(entidade, "Cde", valorString2Novo);

            // assert
            var entidadeAtualizada = sut.Get(valorString);
            Assert.NotNull(entidadeAtualizada);
            Assert.AreEqual(valorString, entidadeAtualizada.Abc);
            Assert.AreEqual(valorInt, entidadeAtualizada.Bar);
            Assert.AreEqual(valorString2Novo, entidadeAtualizada.Cde);
        }
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
