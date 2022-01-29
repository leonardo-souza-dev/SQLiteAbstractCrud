using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste2Campos_StrPk_Int
{
    public class Teste2Campos_DateTimePk_Bool_RepositoryTests
    {
        private string _caminhoArquivoDb;

        [SetUp]
        public void Init()
        {
            _caminhoArquivoDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            var repo = new Teste2Campos_DateTimePk_Bool_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Teste2Campos_DateTimePk_Bool_Repository(_caminhoArquivoDb);
            repo.DropTable();
        }

        [Test]
        public void DeveAtualizar()
        {
            // arrange
            var campoDateTime = DateTime.Now;
            var campoBool = true;
            var entidade = new Teste2Campos_DateTimePk_Bool(campoDateTime, campoBool);
            var sut = new Teste2Campos_DateTimePk_Bool_Repository(_caminhoArquivoDb);
            sut.Insert(entidade);
            var novoValorCampoBool = false;

            // act
            _ = sut.Update(entidade, "CampoBool", novoValorCampoBool);

            // assert
            var entidadeInserida = sut.Get(campoDateTime);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(campoDateTime.Year, entidadeInserida.CampoDateTime.Year);
            Assert.AreEqual(campoDateTime.Month, entidadeInserida.CampoDateTime.Month);
            Assert.AreEqual(campoDateTime.Day, entidadeInserida.CampoDateTime.Day);
            Assert.AreEqual(campoDateTime.Hour, entidadeInserida.CampoDateTime.Hour);
            Assert.AreEqual(campoDateTime.Minute, entidadeInserida.CampoDateTime.Minute);
            Assert.AreEqual(campoDateTime.Second, entidadeInserida.CampoDateTime.Second);
            Assert.AreEqual(campoDateTime.Millisecond, entidadeInserida.CampoDateTime.Millisecond);
            Assert.AreEqual(novoValorCampoBool, entidadeInserida.CampoBool);
        }
    }
}
