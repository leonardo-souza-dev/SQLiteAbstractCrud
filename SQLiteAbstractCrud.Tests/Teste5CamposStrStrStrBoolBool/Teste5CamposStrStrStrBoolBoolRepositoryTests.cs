using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste5CamposStrStrStrBoolBool
{
    public class Teste5CamposStrStrStrBoolBoolRepositoryTests
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
            var repo = new Teste5CamposStrStrStrBoolBoolRepository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverTresCamposStringEDoisBool_DeveObter()
        {
            // arrange
            const string valor1 = "asb";
            const string valor2 = "123";
            const string valor3 = "qwerty";
            const bool valor4 = true;
            const bool valor5 = false;
            var sut = new Teste5CamposStrStrStrBoolBoolRepository(_caminhoArquivoDb);
            sut.Insert(new Teste5CamposStrStrStrBoolBool(valor1, valor2, valor3, valor4, valor5));

            // act
            var result = sut.Get(valor1);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valor1, result.Valor1);
            Assert.AreEqual(valor2, result.Valor2);
            Assert.AreEqual(valor3, result.Valor3);
            Assert.AreEqual(valor4, result.Valor4);
            Assert.AreEqual(valor5, result.Valor5);
        }

        [Test]
        public void QuandoEntidadeTiverTresCamposStringEDoisBool_DeveInserir()
        {
            // arrange
            const string valor1 = "asb";
            const string valor2 = "123";
            const string valor3 = "qwerty";
            const bool valor4 = true;
            const bool valor5 = false;
            var entidade = new Teste5CamposStrStrStrBoolBool(valor1, valor2, valor3, valor4, valor5);
            var sut = new Teste5CamposStrStrStrBoolBoolRepository(_caminhoArquivoDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valor1);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valor1, entidadeInserida.Valor1);
            Assert.AreEqual(valor2, entidadeInserida.Valor2);
            Assert.AreEqual(valor3, entidadeInserida.Valor3);
            Assert.AreEqual(valor4, entidadeInserida.Valor4);
            Assert.AreEqual(valor5, entidadeInserida.Valor5);
        }
    }
}
