using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste1Campo_StrPk
{
    public class Teste1Campo_StrPkRepositoryTests
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
            var repo = new Teste1Campo_StrPkRepository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPk_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            var sut = new Teste1Campo_StrPkRepository(_caminhoArquivoDb);
            sut.Insert(new Teste1Campo_StrPk(valorFoo));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Foo);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPk_DeveInserir()
        {
            // arrange
            const string valorFoo = "fooValor";
            var entidade = new Teste1Campo_StrPk(valorFoo);
            var sut = new Teste1Campo_StrPkRepository(_caminhoArquivoDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorFoo);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorFoo, entidadeInserida.Foo);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPk_DeveObterTudo()
        {
            // arrange
            var entidade1 = new Teste1Campo_StrPk("fooValor1");
            var entidade2 = new Teste1Campo_StrPk("fooValor2");
            var sut = new Teste1Campo_StrPkRepository(_caminhoArquivoDb);
            sut.Insert(entidade1);
            sut.Insert(entidade2);
            
            // act
            var entidadesObtidas = sut.GetAll();

            // assert
            Assert.NotNull(entidadesObtidas);
            Assert.AreEqual(2, entidadesObtidas.Count());
            Assert.True(entidadesObtidas.FirstOrDefault(x => x.Foo == entidade1.Foo) != null);
            Assert.True(entidadesObtidas.FirstOrDefault(x => x.Foo == entidade2.Foo) != null);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPk_DeveInserirEmBatch()
        {
            // arrange
            var valor1 = "fooValor1";
            var valor2 = "fooValor2";
            var valor3 = "fooValor3";
            var entidade1 = new Teste1Campo_StrPk(valor1);
            var entidade2 = new Teste1Campo_StrPk(valor2);
            var entidade3 = new Teste1Campo_StrPk(valor3);
            
            var sut = new Teste1Campo_StrPkRepository(_caminhoArquivoDb);
            
            // act
            sut.InsertBatch(new List<Teste1Campo_StrPk>{ entidade1, entidade2, entidade3 });

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
