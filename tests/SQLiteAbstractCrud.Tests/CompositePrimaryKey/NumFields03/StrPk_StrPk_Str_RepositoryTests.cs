using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields03
{
    public class StrPk_StrPk_Str_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new StrPk_StrPk_Str_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new StrPk_StrPk_Str_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string valorFoo = "fooValor";
            const string valorBar = "123";
            const string valorZab = "zabValor";
            var sut = new StrPk_StrPk_Str_Repository(_pathFileDb);
            sut.Insert(new StrPk_StrPk_Str(valorFoo, valorBar, valorZab));

            // act
            var result = sut.Get(valorFoo);

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(valorFoo, result.Abc);
            Assert.AreEqual(valorBar, result.Bar);
            Assert.AreEqual(valorZab, result.Cde);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string stringValue1 = "stringValue1";
            const string stringValue2 = "stringValue2";
            const string stringValue3 = "stringValue3";
            var entity = new StrPk_StrPk_Str(stringValue1, stringValue2, stringValue3);
            var sut = new StrPk_StrPk_Str_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.Get(stringValue1);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(stringValue1, insertedEntity.Abc);
            Assert.AreEqual(stringValue2, insertedEntity.Bar);
            Assert.AreEqual(stringValue3, insertedEntity.Cde);
        }
    }

    public class StrPk_StrPk_Str_Repository : RepositoryBase<StrPk_StrPk_Str>
    {
        public StrPk_StrPk_Str_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class StrPk_StrPk_Str
    {
        [PrimaryKey]
        public string Abc { get; init; }
        [PrimaryKey]
        public string Bar { get; init; }
        public string Cde { get; init; }

        public StrPk_StrPk_Str(string abc, string bar, string cde)
        {
            Abc = abc;
            Bar = bar;
            Cde = cde;
        }
    }
}
