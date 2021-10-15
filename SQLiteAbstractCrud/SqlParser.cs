using System.Collections.Generic;
using TSQL;
using TSQL.Tokens;

namespace SQLiteAbstractCrud
{
    public static class SqlParser
    {
        public static List<TSQLToken> Parse(string sql)
        {
            return TSQLTokenizer.ParseTokens(sql);
        }
    }
}