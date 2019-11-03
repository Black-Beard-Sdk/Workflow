using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Bb.Dao
{

    public class SqlManager
    {

        public SqlManager(SqlManagerConfiguration configuration)
        {
            _factory = DbProviderFactories.GetFactory(configuration.ProviderInvariantName);
            _configuration = configuration;
            QueryGenerator = configuration.QueryManager(this);

            LogCommandImpl = configuration.LogCommand;

            _regex = new Regex("\\{\\{\\w*}}", RegexOptions.None, TimeSpan.FromMilliseconds(1000));


        }

        public DbProviderFactory Factory => _factory;

        public bool Log { get; private set; }

        public IQueryGenerator QueryGenerator { get; }

        public DbConnectionStringBuilder GetBuilder()
        {
            var builder = _factory.CreateConnectionStringBuilder();
            builder.ConnectionString = _configuration.ConnectionString;
            return builder;
        }

        /// <summary>
        /// Gets the version. 
        /// If the result version equal -1 the query has failed.
        /// If the result version equal -2 the connection has failed
        /// </summary>
        /// <returns></returns>
        public int GetVersion()
        {

            int version = -2;

            if (TestConnection())
            {

                string sql = $"SELECT MAX({QueryGenerator.Member("Version")}) FROM {QueryGenerator.Member("Versions")} ";

                try
                {
                    version = (int)ReadScalar(sql);
                }
                catch (Exception)
                {
                    version = -1;
                }

            }

            return version;

        }

        public void UpdateDatabase(ISgbdUpdater initializer, Dictionary<string, string> arguments, int targetVersion = 0)
        {

            int currentVersion = GetVersion();
            if (currentVersion == -1)
                currentVersion = 0;

            if (currentVersion == -1)
                throw new Exception("Failed to connect database");

            StringBuilder[] scripts = initializer.GetScripts(arguments);

            if (targetVersion <= 0)
                targetVersion = scripts.Length + 1;

            string insertVersion = $"INSERT INTO {QueryGenerator.Member("Versions")} ({QueryGenerator.Member("Version")}) VALUES ({QueryGenerator.ParameterName("version")});";

            while (currentVersion < scripts.Length && targetVersion > currentVersion)
            {

                string sql = scripts[currentVersion].ToString();
                try
                {

                    using (var trans = GetTransaction())
                    {

                        CheckArgumentsAreMapped(currentVersion, sql);

                        var items = sql.Split("-- ----- -----");

                        foreach (var item in items)
                            Update(item);

                        currentVersion++;
                        Update(insertVersion, CreateParameter("version", DbType.Int32, currentVersion));

                        trans.Commit();

                    }

                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to update database to version {currentVersion + 1}", e);
                }

            }

        }

        public void CheckArgumentsAreMapped(int currentVersion, string sql)
        {
            var matches = _regex.Matches(sql);
            if (matches.Count > 0)
            {
                string comma = string.Empty;
                StringBuilder sb = new StringBuilder(100);
                sb.Append("arguments not matched or not specified ('");
                sb.Append(currentVersion);
                foreach (Match item in matches)
                {
                    sb.Append(comma);
                    sb.Append(item.Value);
                    comma = "', '";
                }
                sb.Append("')");

                throw new Exception(sb.ToString());

            }
        }

        public IEnumerable<T> Read<T>(string sql, params DbParameter[] parameters)
            where T : IMapperDbDataReader, new()
        {

            List<T> _results = new List<T>();

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    if (parameters != null)
                        foreach (var parameter in parameters)
                            cmd.Parameters.Add(parameter);

                    var ctx = new DbDataReaderContext();

                    foreach (DbDataReader item in GetReader(cmd))
                    {
                        ctx.Reader = item;
                        T result = new T();
                        result.Map(ctx);
                        _results.Add(result);
                    }

                }

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

            return _results;

        }

        public bool TestConnection(int timeOutInSecond = 20)
        {

            var endTest = DateTime.Now.AddSeconds(timeOutInSecond);

            while (DateTime.Now < endTest)
            {

                try
                {
                    using (var cnx = GetConnection())
                    {
                        cnx.Open();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Thread.Sleep(500);
                }

            }

            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();

            return false;

        }

        public IEnumerable<T> Read<T>(string sql, ObjectMapping mapping, params DbParameter[] parameters)
            where T : new()
        {

            List<T> _results = new List<T>();

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    if (parameters != null)
                        foreach (var parameter in parameters)
                            cmd.Parameters.Add(parameter);

                    var ctx = new DbDataReaderContext();

                    foreach (DbDataReader item in GetReader(cmd))
                    {
                        ctx.Reader = item;
                        T result = new T();
                        mapping.Map<T>(ctx, result);
                        result.Reset(mapping);
                        _results.Add(result);
                    }

                }

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

            return _results;

        }

        public object ReadScalar(string sql, params DbParameter[] parameters)
        {

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    if (parameters != null)
                        foreach (var parameter in parameters)
                            cmd.Parameters.Add(parameter);

                    LogCommand(cmd);

                    return cmd.ExecuteScalar();

                }

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

        }

        public bool Update(string sql, params DbParameter[] Items)
        {
            return Update(sql, Items.ToList());
        }

        public bool Update(string sql, List<DbParameter> Items)
        {

            bool initialized = false;
            DbConnection cnx = null;

            if (_cnx != null)
                cnx = _cnx;
            else
            {
                cnx = GetConnection();
                cnx.Open();
                initialized = true;
            }

            try
            {

                using (var cmd = GetCommand(sql, cnx))
                {

                    foreach (var parameter in Items)
                        cmd.Parameters.Add(parameter);

                    LogCommand(cmd);

                    var result = cmd.ExecuteNonQuery();

                    if (Log || System.Diagnostics.Debugger.IsAttached)
                    {
                        string plurial = result > 1 ? "s" : string.Empty;
                        Trace.WriteLine($"{result} impact{plurial} by query", "Sql");
                    }

                    return result != 0;

                }

            }
            catch (Exception e1)
            {

                Trace.WriteLine($"Sql error {e1.Message}", "Sql");

                if (System.Diagnostics.Debugger.IsAttached)
                    System.Diagnostics.Debugger.Break();

                throw e1;

            }
            finally
            {
                if (initialized)
                    cnx.Dispose();
            }

        }

        public DbParameter CreateParameter(string name, DbType type, object value)
        {
            var p = _factory.CreateParameter();
            p.ParameterName = name;
            //p.ParameterName = QueryGenerator.ParameterName(name);
            p.DbType = type;
            p.Value = value;
            return p;
        }

        public IEnumerable<System.Data.Common.DbDataReader> GetReader(System.Data.Common.DbCommand cmd)
        {
            LogCommand(cmd);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
                yield return reader;

        }

        private readonly Action<DbCommand, IQueryGenerator, StringBuilder> LogCommandImpl;
        private readonly Regex _regex;

        private void LogCommand(DbCommand cmd)
        {

            if (Log || System.Diagnostics.Debugger.IsAttached)
            {
                StringBuilder sb = new StringBuilder(200);
                if (_currentTransaction == null)
                    sb.AppendLine();

                LogCommandImpl(cmd, QueryGenerator, sb);

                Trace.WriteLine(sb.ToString(), "Sql");

            }
        }

        public Transaction GetTransaction(bool autocommit = false)
        {

            if (_currentTransaction != null)
                throw new Exception("connection allready initialized");

            _cnx = GetConnection();
            _cnx.Open();

            return new Transaction(this, autocommit);

        }

        private System.Data.Common.DbCommand GetCommand(string sql, DbConnection cnx)
        {
            var cmd = _factory.CreateCommand();
            cmd.CommandText = sql;
            cmd.Connection = cnx;
            if (_currentTransaction != null)
                cmd.Transaction = _currentTransaction.Current;
            return cmd;
        }


        private DbConnection GetConnection()
        {
            var cnx = _factory.CreateConnection();
            cnx.ConnectionString = _configuration.ConnectionString;
            return cnx;
        }

        public SqlManagerConfiguration Configuration => _configuration;


        private readonly DbProviderFactory _factory;
        private readonly SqlManagerConfiguration _configuration;
        internal DbConnection _cnx;
        internal Transaction _currentTransaction;

    }

    public class Transaction : IDisposable
    {

        public Transaction(SqlManager manager, bool autocommit)
        {
            _commited = false;
            _manager = manager;
            _manager._currentTransaction = this;
            _tranction = _manager._cnx.BeginTransaction();
            _autocommit = autocommit;

            if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                Trace.WriteLine("Begin transaction", "Sql");

        }

        public DbTransaction Current => _tranction;

        public void Commit()
        {

            _tranction.Commit();
            _commited = true;

            if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                Trace.WriteLine("Commit transaction", "Sql");

        }

        public void Dispose()
        {

            if (!_commited)
            {
                if (_autocommit)
                    Commit();

                else
                {
                    if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                        Trace.WriteLine("Rollback transaction", "Sql");
                    _tranction.Rollback();
                }
            }

            _tranction.Dispose();
            _manager._cnx.Dispose();

            _manager._cnx = null;
            _manager._currentTransaction = null;

            if (_manager.Log || System.Diagnostics.Debugger.IsAttached)
                Trace.WriteLine("End transaction", "Sql");

        }

        internal void Rollback()
        {
            _tranction.Rollback();
        }

        private bool _commited;
        private readonly SqlManager _manager;
        private readonly DbTransaction _tranction;
        private readonly bool _autocommit;

    }

}