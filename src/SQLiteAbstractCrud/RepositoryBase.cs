using SQLiteAbstractCrud.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SQLiteAbstractCrud
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        private readonly string _table = typeof(T).Name;
        private readonly string _dataSource;
        private static Fields _fields;

        protected RepositoryBase(string pathDbFile)
        {
            CreateDbFileIfDontExists(pathDbFile);
            _dataSource = $"Data Source={pathDbFile}";
            SetFields();

            if (_fields != null)
            {
                CreateTableIfDontExists(_dataSource);
            }
        }

        protected RepositoryBase()
        {
        }

        public virtual T Insert(T t)
        {
            object rawValue = null;

            using (SQLiteConnection con = new(_dataSource))
            { 
                con.Open();

                var queryInsert = new QueryInsert<T>(t).Raw;

                using (var cmd = new SQLiteCommand(queryInsert, con))
                {
                    cmd.ExecuteNonQuery();
                }

                var pkName = _fields.GetPrimaryKeyName();

                rawValue = t.GetType().GetProperty(pkName).GetValue(t, null);
            }

            var tInserted = Get(rawValue);
            return tInserted;
        }

        private long GetLastInsertedId(SQLiteConnection con)
        {
            using (var cmd = new SQLiteCommand("SELECT last_insert_rowid()", con))
            {
                var foo = cmd.ExecuteScalar();
                return (long)foo;
            }
        }

        public virtual T Update(T t, string field, object value)
        {
            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var query = new QueryUpdate<T>(t, field, value).Raw;

                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }

            return t;
        }

        public virtual void InsertBatch(List<T> list)
        {
            if (!list.Any())
                return;

            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var query = GetQueryInsertBatch(list);
                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            List<T> entities = new();

            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var query = GetQueryGetAll();

                using (var cmd = new SQLiteCommand(query, con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr == null)
                        {
                            Debug.WriteLine("\r\n****\r\n rdr null");
                            Debug.WriteLine(query);
                        }

                        if (rdr.HasRows)
                        {
                            while (rdr.Read())
                            {
                                var entity = Map(rdr);

                                entities.Add(entity);
                            }
                        }
                    }
                }
            }

            return entities;
        }

        public virtual T Get(object id)
        {
            T entity = default;

            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var fieldsNames = _fields.Items.Select(x => x.NameOnDb).ToList();
                var query = GetQueryGet(fieldsNames, id);
                using (var cmd = new SQLiteCommand(query, con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entity = Map(rdr);
                        }
                    }
                }
            }

            return entity;
        }

        public virtual T Get(object id1, object id2)
        {
            T entity = default;

            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var fieldsNames = _fields.Items.Select(x => x.NameOnDb).ToList();
                var query = GetQueryGetComposite(fieldsNames, id1, id2);
                using (var cmd = new SQLiteCommand(query, con))
                {
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entity = Map(rdr);
                        }
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Get records by date (GTE and LTE)
        /// </summary>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="minInclude">Minimum date</param>
        /// <param name="maxInclude">Maximum date</param>
        /// <returns>Records</returns>
        public virtual List<T> GetByDateRange(string fieldName, DateTime minInclude, DateTime maxInclude)
        {
            T entity = default;
            var entities = new List<T>();

            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var fieldsNames = _fields.Items.Select(x => x.NameOnDb).ToList();
                var query = GetQueryDateRange(fieldsNames, fieldName, minInclude, maxInclude);
                using (var cmd = new SQLiteCommand(query, con))
                {

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            entity = Map(rdr);
                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

        public virtual void Delete(object id)
        {
            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var query = GetQueryDelete(id);

                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(object id1, object id2)
        {
            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                var query = GetQueryDeleteComposite(id1, id2);

                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public virtual void DropTable()
        {
            using (SQLiteConnection con = new(_dataSource))
            {
                con.Open();

                using (var cmd = new SQLiteCommand($"DROP TABLE {_table};", con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #region Private Methods

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
                        value = Convert.SqliteDate((DateTime)rawValue);
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
        
        private string GetQueryInsertBatch(List<T> entities)
        {
            var sb = new StringBuilder();
    
            entities.ForEach(entity =>
            {
                var valuesCommas = GetValuesCommas(entity, _fields);

                sb.Append($"({valuesCommas}), ");
            });
    
            var repositoriosValue = sb.ToString().Substring(0, sb.Length - 2);

            var fieldsCommas = GetFieldsCommasFields(_fields.Items.Where(x => !x.IsAutoincrement).ToList());

            var query = $"INSERT OR REPLACE INTO {_table} ({fieldsCommas}) VALUES {repositoriosValue};";
            
            return query;
        }
        
        private string GetQueryDelete(object value)
        {
            var query = $"DELETE FROM {_table} WHERE {_fields.GetPrimaryKeyName()} = {_fields.GetQuotePrimaryKey()}{value.GetValue()}{_fields.GetQuotePrimaryKey()};";
            return query;
        }

        private string GetQueryDeleteComposite(object value1, object value2)
        {
            return $"DELETE FROM {_table} {GetWhereComposite(value1, value2)}";
        }

        private static string GetWhereComposite(object value1, object value2)
        {
            var queryWhere = "";
            var pks = _fields.GetPrimariesKeys().ToList();
            queryWhere += $"WHERE {pks[0].NameOnDb} = {pks[0].Quote}{value1.GetValue()}{pks[0].Quote} AND {pks[1].NameOnDb} = {pks[1].Quote}{value2.GetValue()}{pks[1].Quote}";

            return queryWhere;
        }

        private string GetQueryInsert(string queryValuesAdjust)
        {
            var query = $"INSERT OR REPLACE INTO {_table} " +
                        $"({GetFieldsCommasFields(_fields.Items.Where(x => !x.IsAutoincrement).ToList())}) " +
                        $"VALUES ({queryValuesAdjust});";
            return query;
        }

        private string GetQueryUpdate(T t, string fieldName, object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var setSb = new StringBuilder(" SET ");
            var pkName = _fields.GetPrimaryKeyName();
            var propertyInfo = t.GetType().GetProperty(pkName);
            var pkValue = propertyInfo.GetValue(t, null);

            var pkValueAdjust = AdjustPkValueToQuery(pkValue);

            var valueAdjust = "";
            if (value.GetType().Name.ToLower() == "string")
            {
                valueAdjust = $"'{value}'";
            }
            else if (value.GetType().Name.ToLower() == "int32")
            {
                valueAdjust = value.ToString();
            }
            
            if (string.IsNullOrEmpty(valueAdjust))
            {
                _ = bool.TryParse(value.ToString(), out bool adj);
                valueAdjust = adj ? "1" : "0";
            }

            foreach (var campo in _fields.Items.Select(x => x.NameOnDb).Where(x => x.Equals(fieldName)))
            {
                setSb.Append($"{campo} = {valueAdjust}, ");
            }
            setSb.Remove(setSb.Length - 2, 2);

            var query = $"UPDATE {_table} " +
                        $"{setSb} " +
                        $"WHERE {pkName} = {_fields.GetQuotePrimaryKey()}{pkValueAdjust}{_fields.GetQuotePrimaryKey()} ;";
            return query;
        }

        private static string AdjustPkValueToQuery(object pkValue)
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

        private string GetQueryGetAll()
        {
            var query = $"SELECT {GetFieldsCommas(_fields.Items.Select(x => x.NameOnDb).ToList())} FROM {_table};";

            return query;
        }

        private string GetQueryCreate()
        {
            var fieldsQuery = _fields.Items.Aggregate("", (current, property) => current + $"{property.NameOnDb} {property.TypeSQLite} NOT NULL,");
            var fieldPk = _fields.Items.Where(x => x.IsPrimaryKey).Select(x => x.NameOnDb).ToList();
            var hasFieldAutoincrement = _fields.Items.Any(x => x.IsAutoincrement);

            if (fieldPk == null || !fieldPk.Any())
                throw new AggregateException("Can't find any primary key");

            if (fieldPk.Count > 1 && hasFieldAutoincrement)
                throw new AggregateException("Can't create table with autoincrement field and composite primary key");

            var queryCreate = $"CREATE TABLE if not exists {_table} ( {fieldsQuery} PRIMARY KEY({GetFieldsCommas(fieldPk)} {(hasFieldAutoincrement ? "AUTOINCREMENT" : "")}))";
            
            return queryCreate;
        }

        private static void SetFields()
        {
            _fields = new Fields();
            
            foreach (var t in typeof(T).GetProperties().OrderBy(x => x.Name))
            {
                var primaryKeyAttribute = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                var autoincrementAttribute = t.GetCustomAttributes(typeof(AutoIncrementAttribute), true);

                _fields.AddField(t.Name, t.PropertyType.Name, primaryKeyAttribute.Any(), autoincrementAttribute.Any());
            }
        }
        
        private static void CreateDbFileIfDontExists(string dbFile)
        {
            if (!File.Exists(dbFile))
            {
                var pathSplit = dbFile.Split("/");
                var folderPath = string.Join("/", pathSplit.Take(pathSplit.Length - 1).ToArray());
                
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                SQLiteConnection.CreateFile(dbFile);
            }
        }

        private void CreateTableIfDontExists(string dataSource)
        {
            using (var con = new SQLiteConnection(dataSource))
            {
                con.Open();

                using (var cmd = new SQLiteCommand(GetQueryCreate(), con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetQueryGet(List<string> fieldsNames, object value)
        {
            return $"SELECT {GetFieldsCommas(fieldsNames)} FROM {_table} WHERE {_fields.GetPrimaryKeyName()} = {GetQueryWhere(value)}";
        }

        private string GetQueryGetComposite(List<string> fieldsNames, object value1, object value2)
        {
            return $"SELECT {GetFieldsCommas(fieldsNames)} FROM {_table} {GetWhereComposite(value1, value2)}";
        }

        private string GetQueryDateRange(List<string> fieldsNames, string fieldName, DateTime paramMin, DateTime paramMax)
        {
            var query = $"SELECT {GetFieldsCommas(fieldsNames)} FROM {_table} WHERE DATE({fieldName}) >= DATE('{paramMin:yyyy-MM-dd HH:mm:ss.fff}') AND DATE({fieldName}) <= DATE('{paramMax:yyyy-MM-dd HH:mm:ss.fff}') ";
            
            return query;
        }

        private static string GetQueryWhere(object pkValue)
        {
            var pkValueAdjust = AdjustPkValueToQuery(pkValue);

            var quotePk = _fields.GetQuotePrimaryKey();
            return $"{quotePk}{pkValueAdjust}{quotePk}";
        }

        private static T Map(IDataRecord rdr)
        {
            var fieldsCount = _fields.Items.Count;
            
            var objects = new object[fieldsCount];

            for (int i = 0; i < fieldsCount; i++)
            {
                switch (_fields.Items[i].TypeSQLite)
                {
                    case "TEXT":
                        if (_fields.Items[i].TypeCSharp == "DateTime")
                            objects[i] = rdr.GetDateTime(i);
                        else
                            objects[i] = rdr.GetString(i);
                        break;
                    case "INTEGER":
                        if (_fields.Items[i].TypeCSharp == "Boolean")
                            objects[i] = rdr.GetBoolean(i);
                        else
                            objects[i] = rdr.GetInt32(i);
                        break;
                }
            }

            var entity = (T)Activator.CreateInstance(typeof(T), objects);

            return entity;
        }
        
        #endregion Private
    }
}
