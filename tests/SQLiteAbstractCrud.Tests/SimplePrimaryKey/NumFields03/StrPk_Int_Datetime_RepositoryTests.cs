using System;
using System.IO;
using NUnit.Framework;
using SQLiteAbstractCrud.Proxy.Attributes;

namespace SQLiteAbstractCrud.Tests.SimplePrimaryKey.NumFields03
{
    public class StrPk_Int_Datetime_RepositoryTests
    {
        private readonly string _pathFileDb = $"{Directory.GetCurrentDirectory()}/mydb.db";

        [SetUp]
        public void SetUp()
        {
            new StrPk_Int_Datetime_Repository(_pathFileDb).DropTable();
        }

        [TearDown]
        public void TearDown()
        {
            new StrPk_Int_Datetime_Repository(_pathFileDb).DropTable();
        }

        [Test]
        public void MustGet()
        {
            // arrange
            const string stringValue = "fooValue";
            const int intValue = 123;
            var dateTimeValue = new DateTime(2000, 11, 14);
            var sut = new StrPk_Int_Datetime_Repository(_pathFileDb);
            sut.Insert(new StrPk_Int_Datetime(stringValue, dateTimeValue, intValue));

            // act
            var actual = sut.Get(stringValue);

            // assert
            Assert.NotNull(actual);
            Assert.AreEqual(stringValue, actual.AStringField);
            Assert.AreEqual(intValue, actual.BIntField);
            Assert.AreEqual(dateTimeValue, actual.CDateTimeField);
        }

        [Test]
        public void MustGetByRange()
        {
            // arrange
            const string stringValue1 = "fooValue1";
            var dateTimeValue1 = new DateTime(2020, 3, 17);
            const int intValue1 = 123;

            const string stringValue2 = "fooValue2";
            var dateTimeValue2 = new DateTime(2020, 4, 17);
            const int intValue2 = 456;

            const string stringValue3 = "fooValue3";
            var dateTimeValue3 = new DateTime(2020, 5, 17);
            const int intValue3 = 789;

            var sut = new StrPk_Int_Datetime_Repository(_pathFileDb);
            sut.Insert(new StrPk_Int_Datetime(stringValue1, dateTimeValue1, intValue1));
            sut.Insert(new StrPk_Int_Datetime(stringValue2, dateTimeValue2, intValue2));
            sut.Insert(new StrPk_Int_Datetime(stringValue3, dateTimeValue3, intValue3));

            // act
            var result = sut.GetByDateRange("cDateTimeField", new DateTime(2020, 3, 30), new DateTime(2021, 1, 1));

            // assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void MustInsert()
        {
            // arrange
            const string stringValue = "fooValue";
            const int intValue = 123;
            var dateTimeValue = new DateTime(1983, 3, 14);
            var entity = new StrPk_Int_Datetime(stringValue, dateTimeValue, intValue);
            var sut = new StrPk_Int_Datetime_Repository(_pathFileDb);

            // act
            sut.Insert(entity);

            // assert
            var insertedEntity = sut.Get(stringValue);
            Assert.NotNull(insertedEntity);
            Assert.AreEqual(stringValue, insertedEntity.AStringField);
            Assert.AreEqual(intValue, insertedEntity.BIntField);
            Assert.AreEqual(dateTimeValue, insertedEntity.CDateTimeField);
        }
    }

    public class StrPk_Int_Datetime_Repository : RepositoryBase<StrPk_Int_Datetime>
    {
        public StrPk_Int_Datetime_Repository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class StrPk_Int_Datetime
    {
        [PrimaryKey]
        public string AStringField { get; init; }

        public DateTime CDateTimeField { get; init; }

        public int BIntField { get; init; }

        public StrPk_Int_Datetime(string aStringField, DateTime cDateTimeField, int bIntField)
        {
            BIntField = bIntField;
            CDateTimeField = cDateTimeField;
            AStringField = aStringField;
        }
    }
}
