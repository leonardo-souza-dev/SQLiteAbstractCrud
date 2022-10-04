using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace SQLiteAbstractCrud
{
    public class QueryInsert<T> : Query<T>
    {
        public QueryInsert(T type) : base(type)
        {
        }

        public override string ToRaw()
        {
            var queryValuesAdjust = GetValuesCommas(_type, _proxy.Fields);

            var query = $"INSERT OR REPLACE INTO {this.TableName} " +
                        $"({GetFieldsCommasFields(_proxy.Fields.Items.Where(x => !x.IsAutoincrement).ToList())}) " +
                        $"VALUES ({queryValuesAdjust});";

            return query;
        }

        private static string GetValuesCommas(T t, Fields fields)
        {
            var queryValuesAdjust = "";
            var queryValues = "";
            foreach (var field in fields.Items.Where(x => !x.IsAutoincrement))
            {
                var rawValue = t.GetType().GetProperty(field.NameOnDb).GetValue(t, null);
                object value = field.TypeCSharp switch
                {
                    "DateTime" => Convert.SqliteDate((DateTime)rawValue),
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