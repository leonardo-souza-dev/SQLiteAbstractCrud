using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Util;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryCreate<T> : Query<T>
    {
        public QueryCreate() : base(default)
        {
        }

        public override string ToRaw()
        {
            var fieldsQuery = _proxyBase.Fields.Items.Aggregate("", (current, property) => current + $"{property.NameOnDb} {property.TypeSQLite} NOT NULL,");
            var fieldPk = _proxyBase.Fields.Items.Where(x => x.IsPrimaryKey).Select(x => x.NameOnDb).ToList();
            var hasFieldAutoincrement = _proxyBase.Fields.Items.Any(x => x.IsAutoincrement);

            if (fieldPk == null || !fieldPk.Any())
                throw new AggregateException("Can't find any primary key");

            if (fieldPk.Count > 1 && hasFieldAutoincrement)
                throw new AggregateException("Can't create table with autoincrement field and composite primary key");

            var queryCreate = $"CREATE TABLE if not exists {this.TableName} ( {fieldsQuery} PRIMARY KEY({GetFieldsCommas(fieldPk)} {(hasFieldAutoincrement ? "AUTOINCREMENT" : "")}))";

            return queryCreate;
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