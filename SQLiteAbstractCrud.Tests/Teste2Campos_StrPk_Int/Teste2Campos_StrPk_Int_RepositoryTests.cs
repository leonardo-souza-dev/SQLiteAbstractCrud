using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste2Campos_StrPk_Int
{
    public class Teste2Campos_StrPk_Int_RepositoryTests
    {
        private string _caminhoArquivoDb;

        [SetUp]
        public void Init()
        {
            _caminhoArquivoDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Teste2Campos_StrPk_Int_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPkOutroInt_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            const int valorBar = 123;
            var sut = new Teste2Campos_StrPk_Int_Repository(_caminhoArquivoDb);
            sut.Insert(new Teste2Campos_StrPk_Int(valorFoo, valorBar));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Foo);
            Assert.AreEqual(valorBar, result.Bar);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPkOutroInt_DeveInserir()
        {
            // arrange
            const string valorFoo = "fooValor";
            const int valorBar = 123;
            var entidade = new Teste2Campos_StrPk_Int(valorFoo, valorBar);
            var sut = new Teste2Campos_StrPk_Int_Repository(_caminhoArquivoDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorFoo);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorFoo, entidadeInserida.Foo);
            Assert.AreEqual(valorBar, entidadeInserida.Bar);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPkOutroInt_DeveInserirEmBatch()
        {
            // arrange
            var valor1 = "fooValor1";
            var valor2 = "fooValor2";
            var valor3 = "fooValor3";
            var valorInt1 = 1;
            var valorInt2 = 2;
            var valorInt3 = 3;
            var entidade1 = new Teste2Campos_StrPk_Int(valor1, valorInt1);
            var entidade2 = new Teste2Campos_StrPk_Int(valor2, valorInt2);
            var entidade3 = new Teste2Campos_StrPk_Int(valor3, valorInt3);
            
            var sut = new Teste2Campos_StrPk_Int_Repository(_caminhoArquivoDb);
            
            // act
            sut.InsertBatch(new List<Teste2Campos_StrPk_Int>{ entidade1, entidade2, entidade3 });

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
}
