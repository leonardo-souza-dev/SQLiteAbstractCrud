using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Util;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryGetComposite<T> : Query<T>
    {
        private readonly object _value1;
        private readonly object _value2;

        public QueryGetComposite(object value1, object value2) : base(default)
        {
            this._value1 = value1;
            this._value2 = value2;
        }

        public override string ToRaw()
        {
            return $"SELECT {GetFieldsCommas(_proxyBase.GetFieldsNames())} " + 
                $" FROM {this.TableName} {GetWhereComposite(_value1, _value2)}";
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

        private string GetWhereComposite(object value1, object value2)
        {
            var queryWhere = "";
            var pks = _proxyBase.Fields.GetPrimariesKeys().ToList();
            queryWhere += $"WHERE {pks[0].NameOnDb} = {pks[0].Quote}{value1.GetValue()}{pks[0].Quote} AND {pks[1].NameOnDb} = {pks[1].Quote}{value2.GetValue()}{pks[1].Quote}";

            return queryWhere;
        }
    }
}