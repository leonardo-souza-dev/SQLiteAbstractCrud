using System.IO;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields05
{
    public class Test5Fields_StrPk_Str_Str_Bool_Bool_RepositoryTests
    {
        private string _pathFileDb;

        [SetUp]
        public void Init()
        {
            _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";
            var repo = new Test5Fields_StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);
            repo.DropTable();
        }

        [TearDown]
        public void Setup()
        {
            var repo = new Test5Fields_StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);
            repo.DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string valor1 = "asb";
            const string valor2 = "123";
            const string valor3 = "qwerty";
            const bool valor4 = true;
            const bool valor5 = false;
            var sut = new Test5Fields_StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);
            sut.Insert(new Test5Fields_StrPk_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5));

            // act
            var result = sut.Get(valor1);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valor1, result.Field1);
            Assert.AreEqual(valor2, result.Field2);
            Assert.AreEqual(valor3, result.Field3);
            Assert.AreEqual(valor4, result.Field4);
            Assert.AreEqual(valor5, result.Field5);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string valor1 = "asb";
            const string valor2 = "123";
            const string valor3 = "qwerty";
            const bool valor4 = true;
            const bool valor5 = false;
            var entidade = new Test5Fields_StrPk_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5);
            var sut = new Test5Fields_StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);

            // act
            sut.Insert(entidade);

            // assert
            var entidadeInserida = sut.Get(valor1);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valor1, entidadeInserida.Field1);
            Assert.AreEqual(valor2, entidadeInserida.Field2);
            Assert.AreEqual(valor3, entidadeInserida.Field3);
            Assert.AreEqual(valor4, entidadeInserida.Field4);
            Assert.AreEqual(valor5, entidadeInserida.Field5);
        }

        [Test]
        public void MustUpdate()
        {
            // arrange
            const string valor1 = "asb";
            const string valor2 = "123";
            const string valor3 = "qwerty";
            const bool valor4 = true;
            const bool valor5 = false;
            var entidade = new Test5Fields_StrPk_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5);
            var sut = new Test5Fields_StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);
            sut.Insert(entidade);

            // act
            sut.Update(entidade, "Field4", false);

            // assert
            var entidadeInserida = sut.Get(valor1);
            Assert.NotNull(entidadeInserida);
            Assert.AreEqual(valor1, entidadeInserida.Field1);
            Assert.AreEqual(valor2, entidadeInserida.Field2);
            Assert.AreEqual(valor3, entidadeInserida.Field3);
            Assert.AreEqual(false, entidadeInserida.Field4);
            Assert.AreEqual(valor5, entidadeInserida.Field5);
        }
    }

    public class Test5Fields_StrPk_Str_Str_Bool_Bool_Repository : RepositoryBase<Test5Fields_StrPk_Str_Str_Bool_Bool>
    {
        public Test5Fields_StrPk_Str_Str_Bool_Bool_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class Test5Fields_StrPk_Str_Str_Bool_Bool
    {
        [PrimaryKey]
        public string Field1 { get; init; }
        public string Field2 { get; init; }
        public string Field3 { get; init; }
        public bool Field4 { get; init; }
        public bool Field5 { get; init; }

        public Test5Fields_StrPk_Str_Str_Bool_Bool(string valor1, string valor2, string valor3, bool valor4, bool valor5)
        {
            Field1 = valor1;
            Field2 = valor2;
            Field3 = valor3;
            Field4 = valor4;
            Field5 = valor5;
        }
    }
}
