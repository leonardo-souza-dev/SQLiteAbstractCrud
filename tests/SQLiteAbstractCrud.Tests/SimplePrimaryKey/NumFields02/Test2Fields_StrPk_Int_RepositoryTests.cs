using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields02
{
    public class Test2Fields_StrPk_Int_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            var repository = new Test2Fields_StrPk_Int_Repository(_pathFileDb);
            repository.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Test2Fields_StrPk_Int_Repository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string stringValue = "fooValor";
            const int intValue = 123;
            var sut = new Test2Fields_StrPk_Int_Repository(_pathFileDb);
            sut.Insert(new Test2Fields_StrPk_Int(intValue, stringValue));

            // act
            var result = sut.Get(stringValue);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(stringValue, result.Foo);
            Assert.AreEqual(intValue, result.Bar);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string stringValue = "fooValor";
            const int intValue = 123;
            var entity = new Test2Fields_StrPk_Int(intValue, stringValue);
            var sut = new Test2Fields_StrPk_Int_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var entidadeInserida = sut.Get(stringValue);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(stringValue, entidadeInserida.Foo);
            Assert.AreEqual(intValue, entidadeInserida.Bar);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            const string stringValue = "fooValor";
            const int intValue = 124;
            var entity = new Test2Fields_StrPk_Int(intValue, stringValue);
            var sut = new Test2Fields_StrPk_Int_Repository(_pathFileDb);
            sut.Insert(entity);
            var novoValorBar = 445;

            // act
            _ = sut.Update(entity, "Bar", novoValorBar);

            // assert
            var entidadeInserida = sut.Get(stringValue);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(stringValue, entidadeInserida.Foo);
            Assert.AreEqual(novoValorBar, entidadeInserida.Bar);
        }

        [Test]
        public void MustInsertInBatch()
        {
            // arrange
            var valor1 = "fooValor1";
            var valor2 = "fooValor2";
            var valor3 = "fooValor3";
            var valorInt1 = 1;
            var valorInt2 = 2;
            var valorInt3 = 3;
            var entidade1 = new Test2Fields_StrPk_Int(valorInt1, valor1);
            var entidade2 = new Test2Fields_StrPk_Int(valorInt2, valor2);
            var entidade3 = new Test2Fields_StrPk_Int(valorInt3, valor3);

            var sut = new Test2Fields_StrPk_Int_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<Test2Fields_StrPk_Int> { entidade1, entidade2, entidade3 });

            // assert
            var entidadeInserida1 = sut.Get(valor1);
            var entidadeInserida2 = sut.Get(valor2);
            var entidadeInserida3 = sut.Get(valor3);
            Assert.NotNull(entidadeInserida1);
            Assert.NotNull(entidadeInserida2);
            Assert.NotNull(entidadeInserida3);
            Assert.AreEqual(valor1, entidade1.Foo);
            Assert.AreEqual(valor2, entidade2.Foo);
            Assert.AreEqual(valor3, entidade3.Foo);
        }
    }

    public class Test2Fields_StrPk_Int_Repository : RepositoryBase<Test2Fields_StrPk_Int>
    {
        public Test2Fields_StrPk_Int_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test2Fields_StrPk_Int
    {
        public int Bar { get; }
        [PrimaryKey]
        public string Foo { get; }

        public Test2Fields_StrPk_Int(int bar, string foo)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}
