using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class Test1Field_StrPk_RepositoryTests
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
            var repo = new Test1Field_StrPk_Repository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string valorFoo = "fooValor";
            var sut = new Test1Field_StrPk_Repository(_pathFileDb);
            sut.Insert(new Test1Field_StrPk(valorFoo));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Field1);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string valorFoo = "fooValor";
            var entidade = new Test1Field_StrPk(valorFoo);
            var sut = new Test1Field_StrPk_Repository(_pathFileDb);
            
            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valorFoo);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valorFoo, entidadeInserida.Field1);
        }

        [Test]
        public void MustGetAll()
        {
            // arrange
            var entidade1 = new Test1Field_StrPk("fooValor1");
            var entidade2 = new Test1Field_StrPk("fooValor2");
            var sut = new Test1Field_StrPk_Repository(_pathFileDb);
            sut.Insert(entidade1);
            sut.Insert(entidade2);
            
            // act
            var entidadesObtidas = sut.GetAll();

            // assert
            Assert.NotNull(entidadesObtidas);
            Assert.AreEqual(2, entidadesObtidas.Count());
            Assert.True(entidadesObtidas.FirstOrDefault(x => x.Field1 == entidade1.Field1) != null);
            Assert.True(entidadesObtidas.FirstOrDefault(x => x.Field1 == entidade2.Field1) != null);
        }

        [Test]
        public void MustInsertInBatch()
        {
            // arrange
            var valor1 = "fooValor1";
            var valor2 = "fooValor2";
            var valor3 = "fooValor3";
            var entidade1 = new Test1Field_StrPk(valor1);
            var entidade2 = new Test1Field_StrPk(valor2);
            var entidade3 = new Test1Field_StrPk(valor3);
            
            var sut = new Test1Field_StrPk_Repository(_pathFileDb);
            
            // act
            sut.InsertBatch(new List<Test1Field_StrPk>{ entidade1, entidade2, entidade3 });

            // assert
            var entidadeInserida1 = sut.Get(valor1);
            var entidadeInserida2 = sut.Get(valor2);
            var entidadeInserida3 = sut.Get(valor3);
            Assert.NotNull(entidadeInserida1);
            Assert.NotNull(entidadeInserida2);
            Assert.NotNull(entidadeInserida3);
            Assert.AreEqual(valor1, entidade1.Field1);
            Assert.AreEqual(valor2, entidade2.Field1);
            Assert.AreEqual(valor3, entidade3.Field1);
        }
    }

    public class Test1Field_StrPk_Repository : RepositoryBase<Test1Field_StrPk>
    {
        public Test1Field_StrPk_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test1Field_StrPk
    {
        [PrimaryKey]
        public string Field1 { get; }

        public Test1Field_StrPk(string field1)
        {
            Field1 = field1;
        }
    }
}
