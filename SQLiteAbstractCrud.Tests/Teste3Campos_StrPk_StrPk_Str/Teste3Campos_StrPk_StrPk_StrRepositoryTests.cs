using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste3CamposStrPkStrPkStr
{
    public class Teste3Campos_StrPk_StrPk_StrRepositoryTests
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
            var repo = new Teste3Campos_StrPk_StrPk_StrRepository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPkOutroStringPkOutroString_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            const string valorBar = "123";
            const string valorZab = "zabValor";
            var sut = new Teste3Campos_StrPk_StrPk_StrRepository(_caminhoArquivoDb);
            sut.Insert(new Teste3Campos_StrPk_StrPk_Str(valorFoo, valorBar, valorZab));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Foo);
            Assert.AreEqual(valorBar, result.Bar);
            Assert.AreEqual(valorZab, result.Zab);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPkOutroStringPkOutroString_DeveInserir()
        {
            // arrange
            const string valorFoo = "fooValor";
            const string valorBar = "123";
            const string valorZab = "zabValor";
            var entidade = new Teste3Campos_StrPk_StrPk_Str(valorFoo, valorBar, valorZab);
            var sut = new Teste3Campos_StrPk_StrPk_StrRepository(_caminhoArquivoDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorFoo);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorFoo, entidadeInserida.Foo);
            Assert.AreEqual(valorBar, entidadeInserida.Bar);
            Assert.AreEqual(valorZab, entidadeInserida.Zab);
        }
    }
}
