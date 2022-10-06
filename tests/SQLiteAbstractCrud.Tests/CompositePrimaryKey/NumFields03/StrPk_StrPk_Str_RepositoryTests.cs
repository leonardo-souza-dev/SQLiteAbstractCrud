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
            const string abcValue = "abcValue";
            const string cdeValue = "cdeValue";
            const string barValue = "barValue";
            var sut = new StrPk_StrPk_Str_Repository(_pathFileDb);
            sut.Insert(new StrPk_StrPk_Str(abcValue, cdeValue, barValue));

            // act
            var actual = sut.Get(abcValue);

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(abcValue, actual.Abc);
            Assert.AreEqual(barValue, actual.Bar);
            Assert.AreEqual(cdeValue, actual.Cde);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string abcValue = "abcValue";
            const string cdeValue = "cdeValue";
            const string barValue = "barValue";
            var entity = new StrPk_StrPk_Str(abcValue, cdeValue, barValue);
            var sut = new StrPk_StrPk_Str_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.Get(abcValue);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(abcValue, insertedEntity.Abc);
            Assert.AreEqual(barValue, insertedEntity.Bar);
            Assert.AreEqual(cdeValue, insertedEntity.Cde);
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
        public string Cde { get; init; }
        [PrimaryKey]
        public string Bar { get; init; }

        public StrPk_StrPk_Str(string abc, string cde, string bar)
        {
            Abc = abc;
            Cde = cde;
            Bar = bar;
        }
    }
}
