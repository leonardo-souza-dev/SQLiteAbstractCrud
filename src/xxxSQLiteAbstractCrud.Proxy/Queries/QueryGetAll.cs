using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Util;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryGetAll<T> : Query<T>
    {
        public QueryGetAll() : base(default)
        {
        }

        public override string ToRaw()
        {
            var query = $"SELECT {GetFieldsCommas(_proxyBase.Fields.Items.Select(x => x.NameOnDb).ToList())} FROM {this.TableName};";

            return query;
        }

        private static string GetFieldsCommas(List<string> fields)
        {
            var queryFields = "";
            foreach (var field in fields)
            {
                queryFields += $"{field},";
            }

            var queryFieldsAdjust = queryFields.Substring(0, queryFields.Length - 1);

            return queryFieldsAdjust;
        }
    }
}