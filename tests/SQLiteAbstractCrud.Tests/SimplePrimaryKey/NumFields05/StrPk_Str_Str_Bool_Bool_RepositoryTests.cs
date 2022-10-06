using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields05
{
    public class StrPk_Str_Str_Bool_Bool_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string value1 = "asb";
            const string value2 = "123";
            const string value3 = "qwerty";
            const bool value4 = true;
            const bool value5 = false;
            var sut = new StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);
            sut.Insert(new StrPk_Str_Str_Bool_Bool(value1, value2, value3, value4, value5));

            // act
            var result = sut.Get(value1);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(value1, result.Field1);
            Assert.AreEqual(value2, result.Field2);
            Assert.AreEqual(value3, result.Field3);
            Assert.AreEqual(value4, result.Field4);
            Assert.AreEqual(value5, result.Field5);
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
            var entidade = new StrPk_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5);
            var sut = new StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);

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
            var entidade = new StrPk_Str_Str_Bool_Bool(valor1, valor2, valor3, valor4, valor5);
            var sut = new StrPk_Str_Str_Bool_Bool_Repository(_pathFileDb);
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

    public class StrPk_Str_Str_Bool_Bool_Repository : RepositoryBase<StrPk_Str_Str_Bool_Bool>
    {
        public StrPk_Str_Str_Bool_Bool_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class StrPk_Str_Str_Bool_Bool
    {
        [PrimaryKey]
        public string Field1 { get; init; }
        public string Field2 { get; init; }
        public string Field3 { get; init; }
        public bool Field4 { get; init; }
        public bool Field5 { get; init; }

        public StrPk_Str_Str_Bool_Bool(string field1, string field2, string field3, bool field4, bool field5)
        {
            Field1 = field1;
            Field2 = field2;
            Field3 = field3;
            Field4 = field4;
            Field5 = field5;
        }
    }
}
