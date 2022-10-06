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
            const string stringValue = "fooValor";
            const int intValue = 123;
            var sut = new StrPk_Int_Repository(_pathFileDb);
            sut.Insert(new StrPk_Int(intValue, stringValue));

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
            var entity = new StrPk_Int(intValue, stringValue);
            var sut = new StrPk_Int_Repository(_pathFileDb);

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
            var entity = new StrPk_Int(intValue, stringValue);
            var sut = new StrPk_Int_Repository(_pathFileDb);
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
            var entidade1 = new StrPk_Int(valorInt1, valor1);
            var entidade2 = new StrPk_Int(valorInt2, valor2);
            var entidade3 = new StrPk_Int(valorInt3, valor3);

            var sut = new StrPk_Int_Repository(_pathFileDb);

            // act
            sut.InsertBatch(new List<StrPk_Int> { entidade1, entidade2, entidade3 });

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

    public class StrPk_Int_Repository : RepositoryBase<StrPk_Int>
    {
        public StrPk_Int_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class StrPk_Int
    {
        public int Bar { get; }
        [PrimaryKey]
        public string Foo { get; }

        public StrPk_Int(int bar, string foo)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}
