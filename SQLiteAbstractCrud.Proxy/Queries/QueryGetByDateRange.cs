using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Util;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryGetByDateRange<T> : Query<T>
    {
        private readonly object _fieldName;
        private readonly DateTime _paramMin;
        private readonly DateTime _paramMax;

        public QueryGetByDateRange(object fieldName, DateTime paramMin, DateTime paramMax) : base(default)
        {
            this._fieldName = fieldName;
            this._paramMin = paramMin;
            this._paramMax = paramMax;
        }

        public override string ToRaw()
        {
            var query = $"SELECT {GetFieldsCommas(_proxyBase.GetFieldsNames())} " + 
                $" FROM {this.TableName} " + 
                $" WHERE DATE({_fieldName}) >= DATE('{_paramMin:yyyy-MM-dd HH:mm:ss.fff}') AND DATE({_fieldName}) <= DATE('{_paramMax:yyyy-MM-dd HH:mm:ss.fff}') ";

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