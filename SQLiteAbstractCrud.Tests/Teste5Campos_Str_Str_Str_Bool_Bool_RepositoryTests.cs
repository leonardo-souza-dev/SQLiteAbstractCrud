using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class Teste5Campos_Str_Str_Str_Bool_Bool_RepositoryTests
    {
        private string _caminhoArquivoDb;

        [SetUp]
        public void Init()
        {
            _caminhoArquivoDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            var repo = new Teste5Campos_Str_Str_Str_Bool_Bool_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Teste5Campos_Str_Str_Str_Bool_Bool_Repository(_caminhoArquivoDb);
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
            var sut = new Teste5Campos_Str_Str_Str_Bool_Bool_Repository(_caminhoArquivoDb);
            sut.Insert(new Teste5Campos_Str_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5));

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
            var entidade = new Teste5Campos_Str_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5);
            var sut = new Teste5Campos_Str_Str_Str_Bool_Bool_Repository(_caminhoArquivoDb);

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

        [Test]
        public void QuandoEntidadeTiverTresCamposStringEDoisBool_DeveAtualizar()
        {
            // arrange
            const string valor1 = "asb";
            const string valor2 = "123";
            const string valor3 = "qwerty";
            const bool valor4 = true;
            const bool valor5 = false;
            var entidade = new Teste5Campos_Str_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5);
            var sut = new Teste5Campos_Str_Str_Str_Bool_Bool_Repository(_caminhoArquivoDb);
            sut.Insert(entidade);

            // act
            sut.Update(entidade, "Valor4", false);

            // assert
            var entidadeInserida = sut.Get(valor1);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valor1, entidadeInserida.Valor1);
            Assert.AreEqual(valor2, entidadeInserida.Valor2);
            Assert.AreEqual(valor3, entidadeInserida.Valor3);
            Assert.AreEqual(false, entidadeInserida.Valor4);
            Assert.AreEqual(valor5, entidadeInserida.Valor5);
        }
    }

    public class Teste5Campos_Str_Str_Str_Bool_Bool_Repository : RepositoryBase<Teste5Campos_Str_Str_Str_Bool_Bool>
    {
        public Teste5Campos_Str_Str_Str_Bool_Bool_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Teste5Campos_Str_Str_Str_Bool_Bool
    {
        [PrimaryKey]
        public string Valor1 { get; init; }
        public string Valor2 { get; init; }
        public string Valor3 { get; init; }
        public bool Valor4 { get; init; }
        public bool Valor5 { get; init; }

        public Teste5Campos_Str_Str_Str_Bool_Bool(string valor1, string valor2, string valor3, bool valor4, bool valor5)
        {
            Valor1 = valor1;
            Valor2 = valor2;
            Valor3 = valor3;
            Valor4 = valor4;
            Valor5 = valor5;
        }
    }
}
