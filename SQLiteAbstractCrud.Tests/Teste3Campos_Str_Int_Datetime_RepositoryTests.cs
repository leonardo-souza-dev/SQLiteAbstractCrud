using System;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class Teste3Campos_Str_Int_Datetime_RepositoryTests
    {
        private string _caminhoArquivoDb;

        [SetUp]
        public void Init()
        {
            _caminhoArquivoDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

            new Teste3Campos_Str_Int_Datetime_Repository(_caminhoArquivoDb).DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Teste3Campos_Str_Int_Datetime_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroDateTime_DeveObter()
        {
            // arrange
            const string valorString = "fooValor";
            const int valorInt = 123;
            var valorDateTime = new DateTime(2000, 11, 14);
            var sut = new Teste3Campos_Str_Int_Datetime_Repository(_caminhoArquivoDb);
            sut.Insert(new Teste3Campos_Str_Int_Datetime(valorString, valorInt, valorDateTime));

            // act
            var result = sut.Get(valorString);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorString, result.Foo);
            Assert.AreEqual(valorInt, result.Bar);
            Assert.AreEqual(valorDateTime, result.Data);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroDateTime_DeveObterPorRange()
        {
            // arrange
            const string valorString1 = "fooValor1";
            const int valorInt1 = 123;
            var valorDateTime1 = new DateTime(2020, 3, 17);
            const string valorString2 = "fooValor2";
            const int valorInt2 = 456;
            var valorDateTime2 = new DateTime(2020, 4, 17);
            const string valorString3 = "fooValor3";
            const int valorInt3 = 789;
            var valorDateTime3 = new DateTime(2020, 5, 17);
            var sut = new Teste3Campos_Str_Int_Datetime_Repository(_caminhoArquivoDb);
            sut.Insert(new Teste3Campos_Str_Int_Datetime(valorString1, valorInt1, valorDateTime1));
            sut.Insert(new Teste3Campos_Str_Int_Datetime(valorString2, valorInt2, valorDateTime2));
            sut.Insert(new Teste3Campos_Str_Int_Datetime(valorString3, valorInt3, valorDateTime3));

            // act
            var result = sut.GetByDateRange("Data", new DateTime(2020, 3, 30), new DateTime(2021, 1, 1));

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoStringOutroIntOutroDateTime_DeveInserir()
        {
            // arrange
            const string valorString = "fooValor";
            const int valorInt = 123;
            var valorDateTime = new DateTime(1983, 3, 14);
            var entidade = new Teste3Campos_Str_Int_Datetime(valorString, valorInt, valorDateTime);
            var sut = new Teste3Campos_Str_Int_Datetime_Repository(_caminhoArquivoDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorString);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorString, entidadeInserida.Foo);
            Assert.AreEqual(valorInt, entidadeInserida.Bar);
            Assert.AreEqual(valorDateTime, entidadeInserida.Data);
        }
    }

    public class Teste3Campos_Str_Int_Datetime_Repository : RepositoryBase<Teste3Campos_Str_Int_Datetime>
    {
        public Teste3Campos_Str_Int_Datetime_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Teste3Campos_Str_Int_Datetime
    {
        [PrimaryKey]
        public string Foo { get; init; }
        public int Bar { get; init; }
        public DateTime Data { get; init; }

        public Teste3Campos_Str_Int_Datetime(string foo, int bar, DateTime data)
        {
            Foo = foo;
            Bar = bar;
            Data = data;
        }
    }
}
