using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SQLiteAbstractCrud.Util;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryDeleteComposite<T> : Query<T>
    {
        private readonly object _value1;
        private readonly object _value2;

        public QueryDeleteComposite(object value1, object value2) : base(default)
        {
            this._value1 = value1;
            this._value2 = value2;
        }

        public override string ToRaw()
        {
            return $"DELETE FROM {this.TableName} {GetWhereComposite(_value1, _value2)}";
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