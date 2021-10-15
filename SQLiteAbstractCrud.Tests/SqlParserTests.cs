using System;
using System.Linq;
using NUnit.Framework;

namespace SQLiteAbstractCrud.Tests
{
    public class SqlParserTests
    {
        [Test]
        public void DeveParsearSql()
        {
            // arrange & act
            var result = SqlParser.Parse("SELECT NomeRepo FROM RepoGitHub WHERE NomeRepo = 'foo';");
            result.ForEach(x => Console.WriteLine("type: " + x.Type + ",   value: " + x.Text));

            // assert
            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.AreEqual(9, result.Count);
        }
    }
}
