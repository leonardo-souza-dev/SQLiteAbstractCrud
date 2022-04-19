using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste1Campo_StrPk
{
    public class Teste2Campo2_IntPkAi_StrPk_RepositoryTests
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
            Assert.Throws<ApplicationException>(() => new Teste2Campo2_IntPkAi_StrPk_Repository(_caminhoArquivoDb));
        }
    }
}
