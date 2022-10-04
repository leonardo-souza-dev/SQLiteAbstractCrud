using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Util;

namespace SQLiteAbstractCrud.Proxy.Queries
{
    public class QueryDelete<T> : Query<T>
    {
        private readonly object _id;

        public QueryDelete(object id) : base(default)
        {
            this._id = id;
        }

        public override string ToRaw()
        {
            var query = $"DELETE FROM {this.TableName} " + 
                $" WHERE {_proxyBase.GetPrimaryKeyName()} = " + 
                $"{_proxyBase.GetQuotePrimaryKey()}{_id.GetValue()}{_proxyBase.Fields.GetQuotePrimaryKey()};";
            return query;
        }
    }
}