using System;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste3CamposStrStrDatetime
{
    public class Teste3CamposRepositoryTests
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
            var repo = new Teste3CamposStrStrDatetimeRepository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroString_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            const int valorBar = 123;
            var valorData = new DateTime(1983, 3, 14);
            var sut = new Teste3CamposStrStrDatetimeRepository(_caminhoArquivoDb);
            sut.Insert(new Teste3CamposStrStrDatetime(valorFoo, valorBar, valorData));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Foo);
            Assert.AreEqual(valorBar, result.Bar);
            Assert.AreEqual(valorData, result.Data);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroDateTime_DeveInserir()
        {
            // arrange
            const string valorFoo = "fooValor";
            const int valorBar = 123;
            var valorData = new DateTime(1983, 3, 14);
            var entidade = new Teste3CamposStrStrDatetime(valorFoo, valorBar, valorData);
            var sut = new Teste3CamposStrStrDatetimeRepository(_caminhoArquivoDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorFoo);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorFoo, entidadeInserida.Foo);
            Assert.AreEqual(valorBar, entidadeInserida.Bar);
            Assert.AreEqual(valorData, entidadeInserida.Data);
        }
    }
}
