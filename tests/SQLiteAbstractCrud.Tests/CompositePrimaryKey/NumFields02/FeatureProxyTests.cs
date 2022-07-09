using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests.CompositePrimaryKey.NumFields02
{
    public class FeatureProxyTests
    {
        private string _pathDbFile;

        [SetUp]
        public void Init()
        {
            _pathDbFile = $"{Directory.GetCurrentDirectory()}/mydb.db";
        }

        [Test]
        public void MustGetAfterInsert()
        {
            // arrange
            var mmmField = "mmm";
            var zzzField = "zzz";
            var aaaField = "aaa";
            var featureProxyEntity = new FeatureProxyEntity(mmmField, zzzField, aaaField);

            var sut = new FeatureProxyEntityRepository(_pathDbFile);

            // act
            var result = sut.Insert(featureProxyEntity);

            // assert
            var actual = sut.GetAll().First();
            Assert.AreEqual(mmmField, actual.MmmField);
            Assert.AreEqual(zzzField, actual.ZzzField);
            Assert.AreEqual(aaaField, actual.AaaField);
        }
    }

    public class FeatureProxyEntityRepository : RepositoryBase<FeatureProxyEntity>
    {
        public FeatureProxyEntityRepository(string pathDbFile) : base(pathDbFile)
        {
        }
    }

    public class FeatureProxyEntity
    {
        [PrimaryKey]
        public string MmmField { get; }
        public string ZzzField { get; }
        public string AaaField { get; }

        public FeatureProxyEntity(string mmmField, string zzzField, string aaaField)
        {
            MmmField = mmmField;
            ZzzField = zzzField;
            AaaField = aaaField;
        }
    }
}
