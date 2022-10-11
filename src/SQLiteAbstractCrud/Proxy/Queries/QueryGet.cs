using System;
using System.Linq;
using System.Collections.Generic;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryGet<T> : Query<T>
    {
        private readonly object _id;

        public QueryGet(object id) : base(default)
        {
            this._id = id;
        }

        public override string ToRaw()
        {
            var ctorParametersNames = _proxyBase.GetCtorParemetersInfos().Select(x => x.Name).ToList();
            return $" SELECT {GetFieldsCommas(ctorParametersNames)} " + 
                   $" FROM {this.TableName} " + 
                   $" WHERE {_proxyBase.Fields.GetPrimaryKeyName()} = {GetQueryWhere(_id)}";
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

        private string GetQueryWhere(object pkValue)
        {
            var pkValueAdjust = AdjustPkValueToQuery(pkValue);

            var quotePk = _proxyBase.Fields.GetQuotePrimaryKey();
            return $"{quotePk}{pkValueAdjust}{quotePk}";
        }

        private string AdjustPkValueToQuery(object pkValue)
        {
            string newPkValue;

            if (_proxyBase.Fields.GetPrimaryKeyType().ToLower() == "datetime")
            {
                var dateTime = (DateTime)pkValue;
                newPkValue = dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
                newPkValue = pkValue.ToString();

            return newPkValue;
        }
    }
}