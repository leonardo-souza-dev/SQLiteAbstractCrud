using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace SQLiteAbstractCrud
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        private readonly string _table = typeof(T).Name;
        private SQLiteConnection _con;
        private static Fields _fields;

        protected RepositoryBase(string pathDbFile)
        {
            var dataSource = CreateDbFileIfDontExists(pathDbFile);
            SetFields();

            if (_fields != null)
            {
                CreateTableIfDontExists(dataSource);
            }
        }

        protected RepositoryBase()
        {
        }

        public virtual T Insert(T t)
        {
            _con.Open();
            
            var queryValuesAdjust = GetValuesCommas(t, _fields);
            var queryInsert = GetQueryInsert(queryValuesAdjust);

            var cmd = new SQLiteCommand(queryInsert, _con);
            cmd.ExecuteNonQuery();
            
            _con.Close();
            
            return t;
        }

        public virtual void Update(T t)
        {
            throw new NotImplementedException();
        }

        public virtual void InsertBatch(List<T> list)
        {
            _con.Open();
            var query = GetQueryInsertBatch(list);
            new SQLiteCommand(query, _con).ExecuteNonQuery();
            _con.Close();
        }

        public virtual IEnumerable<T> GetAll()
        {
            List<T> entities = new();

            _con.Open();
            var cmd = new SQLiteCommand(GetQueryGetAll(), _con);
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var entity = Map(rdr);

                    entities.Add(entity);
                }
            }
            _con.Close();

            return entities;
        }

        public virtual T Get(object id)
        {
            T entity = default;

            _con.Open();
            var fieldsNames = _fields.Itens.Select(x => x.Name).ToList();
            var cmd = new SQLiteCommand(GetQueryGet(fieldsNames, id), _con);

            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    entity = Map(rdr);
                }
            }

            _con.Close();

            return entity;
        }

        public virtual List<T> GetByDateRange(string fieldName, object minInclude, object maxInclude)
        {
            T entity = default;

            _con.Open();
            var fieldsNames = _fields.Itens.Select(x => x.Name).ToList();
            var cmd = new SQLiteCommand(GetQueryDateRange(fieldsNames, fieldName, minInclude, maxInclude), _con);

            var entities = new List<T>();

            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    entity = Map(rdr);
                    entities.Add(entity);
                }
            }

            _con.Close();

            return entities;
        }

        public virtual void Delete(object id)
        {
            _con.Open();

            var cmd = new SQLiteCommand(GetQueryDelete(id), _con);
            cmd.ExecuteNonQuery();

            _con.Close();
        }

        public virtual void DropTable()
        {
            _con.Open();

            var cmd = new SQLiteCommand($"DROP TABLE {_table};", _con);
            cmd.ExecuteNonQuery();

            _con.Close();
        }

        #region Private Methods

        private static string GetValuesCommas(T t, Fields fields)
        {
            var queryValuesAdjust = "";
            var queryValues = "";
            foreach (var field in fields.Itens)
            {
                var rawValue = t.GetType().GetProperty(field.Name).GetValue(t, null);
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

            var query = $"INSERT OR REPLACE INTO {_table} ({GetFieldsCommas(_fields.Itens.Select(x => x.Name).ToList())}) VALUES {repositoriosValue};";
            
            return query;
        }
        
        private string GetQueryDelete(object valor)
        {
            return $"DELETE FROM {_table} WHERE {_fields.GetPrimaryKeyName()} = {_fields.GetQuotePrimaryKey()}{valor}{_fields.GetQuotePrimaryKey()};";
        }

        private string GetQueryInsert(string queryValuesAdjust)
        {
            var query = $"INSERT OR REPLACE INTO {_table} " +
                        $"({GetFieldsCommas(_fields.Itens.Select(x => x.Name).ToList())}) " +
                        $"VALUES ({queryValuesAdjust});";
            return query;
        }
        
        private static string GetFieldsCommas(List<string> fields)
        {
            var queryFields = "";
            foreach (var field in fields)
            {
                queryFields = queryFields + $"{field},";
            }

            var queryFieldsAdjust = queryFields.Substring(0, queryFields.Length - 1);

            return queryFieldsAdjust;
        }

        private string GetQueryGetAll()
        {
            var query = $"SELECT {GetFieldsCommas(_fields.Itens.Select(x => x.Name).ToList())} FROM {_table};";

            return query;
        }

        private string GetQueryCreate()
        {
            var fieldsQuery = _fields.Itens.Aggregate("", (current, property) => current + $"{property.Name} {property.TypeSQLite} NOT NULL,");
            var fieldPk = _fields.Itens.Where(x => x.IsPrimaryKey).Select(x => x.Name).ToList();

            if (fieldPk == null || !fieldPk.Any())
                throw new ApplicationException("Não foi encontrada nenhuma Primary Key na entidade");
            
            var queryCreate = $"CREATE TABLE if not exists {_table} ( {fieldsQuery} PRIMARY KEY({GetFieldsCommas(fieldPk)}))";
            
            return queryCreate;
        }

        private void SetFields()
        {
            _fields = new Fields();
            
            foreach (var t in typeof(T).GetProperties())
            {
                var primaryKeyAttribute = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                
                _fields.AdicionarCampo(t.Name, t.PropertyType.Name, primaryKeyAttribute.Any());
            }
        }
        
        private static string CreateDbFileIfDontExists(string dbFile)
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
            
            var dataSource = $"Data Source={dbFile}";
            return dataSource;
        }

        private void CreateTableIfDontExists(string dataSource)
        {
            _con = new SQLiteConnection(dataSource);

            _con.Open();
            var cmdCreateTable = new SQLiteCommand(GetQueryCreate(), _con);
            cmdCreateTable.ExecuteNonQuery();
            _con.Close();
        }

        private string GetQueryGet(List<string> fieldsNames, object value)
        {
            return $"SELECT {GetFieldsCommas(fieldsNames)} FROM {_table} WHERE {GetPrimaryKeyName()} = {GetQueryWhere(value)}";
        }

        private string GetQueryDateRange(List<string> fieldsNames, string fieldName, object paramMin, object paramMax)
        {
            var query = $"SELECT {GetFieldsCommas(fieldsNames)} FROM {_table} WHERE {fieldName} > '{paramMin:yyyy-MM-dd 00:00:00.000}' AND {fieldName} < '{paramMax:yyyy-MM-dd 23:59:59.999}'";
            
            return query;
        }

        private string GetQueryWhere(object id)
        {
            var quotePk = _fields.GetQuotePrimaryKey();
            return $"{quotePk}{id}{quotePk}";
        }

        private string GetPrimaryKeyName()
        {
            return _fields.GetPrimaryKeyName();
        }

        private T Map(IDataRecord rdr)
        {
            var fieldsCount = _fields.Itens.Count;
            
            var objects = new object[fieldsCount];

            for (int i = 0; i < fieldsCount; i++)
            {
                switch (_fields.Itens[i].TypeSQLite)
                {
                    case "TEXT":
                        if (_fields.Itens[i].TypeCSharp == "DateTime")
                            objects[i] = rdr.GetDateTime(i);
                        else
                            objects[i] = rdr.GetString(i);
                        break;
                    case "INTEGER":
                        if (_fields.Itens[i].TypeCSharp == "Boolean")
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
