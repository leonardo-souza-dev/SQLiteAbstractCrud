using SQLiteAbstractCrud.Proxy;
using SQLiteAbstractCrud.Proxy.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SQLiteAbstractCrud
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        private readonly string _table = typeof(T).Name;
        private readonly string _dataSource;
        private readonly ProxyBase _proxyBase = new (typeof(T));

        protected RepositoryBase(string pathDbFile)
        {
            CreateDbFileIfDontExists(pathDbFile);
            _dataSource = $"Data Source={pathDbFile}";
            
            CreateTableIfDontExists(_dataSource);            
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

                var pkName = _proxyBase.GetPrimaryKeyName();

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

                var query = new QueryInsertBatch<T>(list).Raw;

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

                var query = new QueryGetAll<T>().Raw;

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

                var query = new QueryGet<T>(id).Raw;

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

                var fieldsNames = _proxyBase.GetFieldsNames();

                var query = new QueryGetComposite<T>(id1, id2).Raw;

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

                var query = new QueryGetByDateRange<T>(fieldName, minInclude, maxInclude).Raw;

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

                var query = new QueryDelete<T>(id).Raw;

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

                var query = new QueryDeleteComposite<T>(id1, id2).Raw;

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

                var query = new QueryCreate<T>().Raw;

                using (var cmd = new SQLiteCommand(query, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private T Map(IDataRecord rdr)
        {
            var args = GetArgs(rdr);
            var entity = (T)Activator.CreateInstance(typeof(T), args);

            return entity;
        }

        private object[] GetArgs(IDataRecord rdr)
        {
            var fieldsCount = _proxyBase.GetFieldsCount();

            var objects = new object[fieldsCount];

            for (int i = 0; i < fieldsCount; i++)
            {
                switch (_proxyBase.GetFieldTypeSQLite(i))
                {
                    case "TEXT":
                        if (_proxyBase.GetFieldTypeCSharp(i) == "DateTime")
                            objects[i] = rdr.GetDateTime(i);
                        else
                            objects[i] = rdr.GetString(i);
                        break;
                    case "INTEGER":
                        if (_proxyBase.GetFieldTypeCSharp(i) == "Boolean")
                            objects[i] = rdr.GetBoolean(i);
                        else
                            objects[i] = rdr.GetInt32(i);
                        break;
                }
            }

            return objects;
        }

        #endregion Private
    }
}
