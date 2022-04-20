using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class Teste1Campo_StrPk_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Teste1Campo_StrPkRepository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPk_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            var sut = new Teste1Campo_StrPkRepository(_pathFileDb);
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
            var sut = new Teste1Campo_StrPkRepository(_pathFileDb);
            
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
            var sut = new Teste1Campo_StrPkRepository(_pathFileDb);
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
            
            var sut = new Teste1Campo_StrPkRepository(_pathFileDb);
            
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

    public class Teste1Campo_StrPkRepository : RepositoryBase<Teste1Campo_StrPk>
    {
        public Teste1Campo_StrPkRepository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Teste1Campo_StrPk
    {
        [PrimaryKey]
        public string Foo { get; }

        public Teste1Campo_StrPk(string foo)
        {
            Foo = foo;
        }
    }
}
