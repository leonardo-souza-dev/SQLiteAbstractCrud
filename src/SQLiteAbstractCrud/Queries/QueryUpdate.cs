using System;
using System.Linq;
using System.Text;

namespace SQLiteAbstractCrud.Queries
{
    public class QueryUpdate<T> : Query<T>
    {
        private readonly object _value;
        private readonly string _fieldName;

        public QueryUpdate(T type, string fieldName, object value) : base(type)
        {
            _value = value;
            _fieldName = fieldName;
        }

        public override string ToRaw()
        {
            if (_value == null)
                throw new ArgumentNullException(nameof(_value));

            var setSb = new StringBuilder(" SET ");
            var pkName = _fields.GetPrimaryKeyName();
            var propertyInfo = _type.GetType().GetProperty(pkName);
            var pkValue = propertyInfo.GetValue(_type, null);

            var pkValueAdjust = AdjustPkValueToQuery(pkValue);

            var valueAdjust = "";
            if (_value.GetType().Name.ToLower() == "string")
            {
                valueAdjust = $"'{_value}'";
            }
            else if (_value.GetType().Name.ToLower() == "int32")
            {
                valueAdjust = _value.ToString();
            }

            if (string.IsNullOrEmpty(valueAdjust))
            {
                _ = bool.TryParse(_value.ToString(), out bool adj);
                valueAdjust = adj ? "1" : "0";
            }

            foreach (var campo in _fields.Items.Select(x => x.NameOnDb).Where(x => x.Equals(_fieldName)))
            {
                setSb.Append($"{campo} = {valueAdjust}, ");
            }
            setSb.Remove(setSb.Length - 2, 2);

            var query = $"UPDATE {this.TableName} " +
                        $"{setSb} " +
                        $"WHERE {pkName} = {_fields.GetQuotePrimaryKey()}{pkValueAdjust}{_fields.GetQuotePrimaryKey()} ;";

            return query;
        }


        private string AdjustPkValueToQuery(object pkValue)
        {
            string newPkValue;

            if (_fields.GetPrimaryKeyType().ToLower() == "datetime")
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