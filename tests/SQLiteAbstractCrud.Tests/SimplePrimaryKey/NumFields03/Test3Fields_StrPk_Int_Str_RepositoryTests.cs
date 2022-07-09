using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields03
{
    public class Test3Fields_StrPk_Int_Str_RepositoryTests
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
            var repo = new Teste3Campos_StrPk_Int_Str_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroString_DeveObter()
        {
            // arrange
            const string valorFoo = "fooValor";
            const int valorBar = 123;
            const string valorAsdfg = "qwerty";
            var sut = new Teste3Campos_StrPk_Int_Str_Repository(_caminhoArquivoDb);
            sut.Insert(new Teste3Campos_StrPk_Int_Str(valorFoo, valorBar, valorAsdfg));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Abc);
            Assert.AreEqual(valorBar, result.Bar);
            Assert.AreEqual(valorAsdfg, result.Cde);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroString_DeveInserir()
        {
            // arrange
            const string valorFoo = "fooValor";
            const int valorBar = 123;
            const string valorAsdfg = "qwerty";
            var entidade = new Teste3Campos_StrPk_Int_Str(valorFoo, valorBar, valorAsdfg);
            var sut = new Teste3Campos_StrPk_Int_Str_Repository(_caminhoArquivoDb);

            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorFoo);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorFoo, entidadeInserida.Abc);
            Assert.AreEqual(valorBar, entidadeInserida.Bar);
            Assert.AreEqual(valorAsdfg, entidadeInserida.Cde);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroStringDeveInserirEmBatch()
        {
            // arrange
            var valorStrPk1 = "fooValor1";
            var valorStrPk2 = "fooValor2";
            var valorStrPk3 = "fooValor3";
            var valorInt1 = 1;
            var valorInt2 = 2;
            var valorInt3 = 3;
            var valorStr1 = "fooValor1";
            var valorStr2 = "fooValor2";
            var valorStr3 = "fooValor3";
            var entidade1 = new Teste3Campos_StrPk_Int_Str(valorStrPk1, valorInt1, valorStr1);
            var entidade2 = new Teste3Campos_StrPk_Int_Str(valorStrPk2, valorInt2, valorStr2);
            var entidade3 = new Teste3Campos_StrPk_Int_Str(valorStrPk3, valorInt3, valorStr3);

            var sut = new Teste3Campos_StrPk_Int_Str_Repository(_caminhoArquivoDb);

            // act
            sut.InsertBatch(new List<Teste3Campos_StrPk_Int_Str> { entidade1, entidade2, entidade3 });

            // assert
            var entidadeInserida1 = sut.Get(valorStrPk1);
            var entidadeInserida2 = sut.Get(valorStrPk2);
            var entidadeInserida3 = sut.Get(valorStrPk3);
            Assert.NotNull(entidadeInserida1);
            Assert.NotNull(entidadeInserida2);
            Assert.NotNull(entidadeInserida3);
            Assert.AreEqual(valorStrPk1, entidade1.Abc);
            Assert.AreEqual(valorStrPk2, entidade2.Abc);
            Assert.AreEqual(valorStrPk3, entidade3.Abc);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroString_DeveAtualizar()
        {
            // arrange
            const string valorString = "valorString";
            const int valorInt = 123;
            const string valorString2 = "valorString2";
            const string valorString2Novo = "valorString2novo";
            var entidade = new Teste3Campos_StrPk_Int_Str(valorString, valorInt, valorString2);
            var sut = new Teste3Campos_StrPk_Int_Str_Repository(_caminhoArquivoDb);
            sut.Insert(entidade);

            // act
            _ = sut.Update(entidade, "Cde", valorString2Novo);

            // assert
            var entidadeAtualizada = sut.Get(valorString);
            Assert.NotNull(entidadeAtualizada);
            Assert.AreEqual(valorString, entidadeAtualizada.Abc);
            Assert.AreEqual(valorInt, entidadeAtualizada.Bar);
            Assert.AreEqual(valorString2Novo, entidadeAtualizada.Cde);
        }
    }

    public class Teste3Campos_StrPk_Int_Str_Repository : RepositoryBase<Teste3Campos_StrPk_Int_Str>
    {
        public Teste3Campos_StrPk_Int_Str_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Teste3Campos_StrPk_Int_Str
    {
        [PrimaryKey]
        public string Abc { get; init; }
        public int Bar { get; init; }
        public string Cde { get; init; }

        public Teste3Campos_StrPk_Int_Str(string abc, int bar, string cde)
        {
            Abc = abc;
            Bar = bar;
            Cde = cde;
        }
    }
}
