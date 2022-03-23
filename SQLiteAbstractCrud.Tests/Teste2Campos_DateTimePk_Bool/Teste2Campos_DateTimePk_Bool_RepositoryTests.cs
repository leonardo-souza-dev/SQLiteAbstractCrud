using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.Teste2Campos_StrPk_Int
{
    public class Teste2Campos_DateTimePk_Bool_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            var repo = new Teste2Campos_DateTimePk_Bool_Repository(_pathFileDb);
            repo.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Teste2Campos_DateTimePk_Bool_Repository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void GivenAnEntity_WhenUpdate_ThenPropValuesAreCorrect()
        {
            // arrange
            var campoDateTime = DateTime.Now;
            var campoBool = true;
            var entidade = new Teste2Campos_DateTimePk_Bool(campoDateTime, campoBool);
            var sut = new Teste2Campos_DateTimePk_Bool_Repository(_pathFileDb);
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

        [Test]
        public void GivenAnExistingItensInDb_WhenGetAll_ThenMustGet()
        {
            // arrange
            var entity1 = new Teste2Campos_DateTimePk_Bool(new DateTime(2000, 1, 10), true);
            var entity2 = new Teste2Campos_DateTimePk_Bool(new DateTime(2000, 1, 20), false);
            var sut = new Teste2Campos_DateTimePk_Bool_Repository(_pathFileDb);
            sut.InsertBatch(new List<Teste2Campos_DateTimePk_Bool> { entity1, entity2 });

            // act
            var actual = sut.GetAll();

            // assert
            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(10, actual.FirstOrDefault(x => x.CampoBool).CampoDateTime.Day);
            Assert.AreEqual(20, actual.FirstOrDefault(x => !x.CampoBool).CampoDateTime.Day);
        }

        [Test]
        public void GivenAnEmptyDb_WhenGetAll_ThenMustGetNothing()
        {
            // arrange
            var sut = new Teste2Campos_DateTimePk_Bool_Repository(_pathFileDb);

            // act
            var actual = sut.GetAll();

            // assert
            Assert.AreEqual(0, actual.Count());
        }
    }
}
