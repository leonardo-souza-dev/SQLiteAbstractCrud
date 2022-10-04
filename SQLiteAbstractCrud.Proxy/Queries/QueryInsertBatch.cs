using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryInsertBatch<T> : Query<T>
    {
        private List<T> _entities;

        public QueryInsertBatch(List<T> entities) : base(entities[0])
        {
            _entities = entities;
        }

        public override string ToRaw()
        {
            var sb = new StringBuilder();

            _entities.ForEach(entity =>
            {
                var valuesCommas = GetValuesCommas(entity, _proxyBase.Fields);

                sb.Append($"({valuesCommas}), ");
            });

            var repositoriosValue = sb.ToString().Substring(0, sb.Length - 2);

            var fieldsCommas = GetFieldsCommasFields(_proxyBase.GetFieldsNotAutoIncrement());

            var query = $"INSERT OR REPLACE INTO {this.TableName} ({fieldsCommas}) VALUES {repositoriosValue};";

            return query;
        }

        private static string GetValuesCommas(T t, Fields fields)
        {
            var queryValuesAdjust = "";
            var queryValues = "";
            foreach (var field in fields.Items.Where(x => !x.IsAutoincrement))
            {
                var rawValue = t.GetType().GetProperty(field.NameOnDb).GetValue(t, null);
                object value = "";
                switch (field.TypeCSharp)
                {
                    case "DateTime":
                        value = Util.Convert.SqliteDate((DateTime)rawValue);
                        break;
                    case "Boolean":
                        value = ((bool)rawValue) ? 1 : 0;
                        break;
                    default:
                        value = rawValue.ToString().Replace('"', '\'').Replace("'", "''");
                        break;
                }

                queryValues += $"{field.Quote}{value}{field.Quote},";
            }
            queryValuesAdjust = queryValues.Substring(0, queryValues.Length - 1);

            return queryValuesAdjust;
        }

        private static string GetFieldsCommasFields(List<Field> fieldsIsNotAutoincrement)
        {
            var queryFields = "";
            foreach (Field fieldIsNotAutoincrement in fieldsIsNotAutoincrement)
            {
                queryFields += $"{fieldIsNotAutoincrement.NameOnDb},";
            }

            var queryFieldsAdjust = queryFields.Substring(0, queryFields.Length - 1);

            return queryFieldsAdjust;
        }

    }
}