using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class Teste2Campos_IntPkAi_StrPk_RepositoryTests
    {
        private string _caminhoArquivoDb;

        [SetUp]
        public void Init()
        {
            _caminhoArquivoDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
        }

        [Test]
        public void QuandoEntidadeTiverUmCampoIntPkAiEPkComposta_DeveObterExcecao()
        {
            // arrange, act & assert
            Assert.Throws<ApplicationException>(() => new Teste2Campos_IntPkAi_StrPk_Repository(_caminhoArquivoDb));
        }
    }

    public class Teste2Campos_IntPkAi_StrPk_Repository : RepositoryBase<Teste2Campos_IntPkAi_StrPk>
    {
        public Teste2Campos_IntPkAi_StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Teste2Campos_IntPkAi_StrPk
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Foo { get; }

        [PrimaryKey]
        public string OutroCampo { get; }

        public Teste2Campos_IntPkAi_StrPk(int foo, string outroCampo)
        {
            Foo = foo;
            OutroCampo = outroCampo;
        }
    }
}
