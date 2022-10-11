using System;
using System.Linq;
using System.Collections.Generic;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryInsert<T> : Query<T>
    {
        public QueryInsert(T type) : base(type)
        {
        }

        public override string ToRaw()
        {
            var queryValuesAdjust = GetValuesCommas(_proxyBase.Fields);

            var query = $"INSERT OR REPLACE INTO {this.TableName} " +
                        //$"({GetFieldsCommasFields(_proxyBase.Fields.Items.Where(x => !x.IsAutoincrement).ToList())}) " +
                        $"({GetFieldsCommasFields(_proxyBase.GetFieldsNotAutoIncrement())}) " +
                        $"VALUES ({queryValuesAdjust});";

            return query;
        }

        private string GetValuesCommas(Fields fields)
        {
            var queryValuesAdjust = "";
            var queryValues = "";
            foreach (var field in fields.Items.Where(x => !x.IsAutoincrement))
            {
                var rawValue = GetRawValue(field.NameOnDb);
                object value = field.TypeCSharp switch
                {
                    "DateTime" => Util.Convert.SqliteDate((DateTime)rawValue),
                    "Boolean" => ((bool)rawValue) ? 1 : 0,
                    _ => rawValue.ToString().Replace('"', '\'').Replace("'", "''"),
                };
                queryValues += $"{field.Quote}{value}{field.Quote},";
            }
            queryValuesAdjust = queryValues.Substring(0, queryValues.Length - 1);

            return queryValuesAdjust;
        }

        private static string GetFieldsCommasFields(List<Field> fields)
        {
            var queryFields = "";
            foreach (Field field in fields)
            {
                queryFields += $"{field.NameOnDb},";
            }

            var queryFieldsAdjust = queryFields.Substring(0, queryFields.Length - 1);

            return queryFieldsAdjust;
        }
    }
}