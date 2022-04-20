using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class Teste3Campos_StrPk_StrPk_Str_RepositoryTests
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
            var repo = new Teste3Campos_StrPk_StrPk_Str_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringPkOutroStringPkOutroString_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            const string valorBar = "123";
            const string valorZab = "zabValor";
            var sut = new Teste3Campos_StrPk_StrPk_Str_Repository(_caminhoArquivoDb);
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
            var sut = new Teste3Campos_StrPk_StrPk_Str_Repository(_caminhoArquivoDb);
            
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

    public class Teste3Campos_StrPk_StrPk_Str_Repository : RepositoryBase<Teste3Campos_StrPk_StrPk_Str>
    {
        public Teste3Campos_StrPk_StrPk_Str_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Teste3Campos_StrPk_StrPk_Str
    {
        [PrimaryKey]
        public string Foo { get; init; }
        [PrimaryKey]
        public string Bar { get; init; }
        public string Zab { get; init; }

        public Teste3Campos_StrPk_StrPk_Str(string foo, string bar, string zab)
        {
            Foo = foo;
            Bar = bar;
            Zab = zab;
        }
    }
}
