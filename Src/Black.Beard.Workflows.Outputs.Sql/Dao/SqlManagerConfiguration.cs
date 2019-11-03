using System;
using System.ComponentModel;
using System.Data.Common;
using System.Text;

namespace Bb.Dao
{

    public class SqlManagerConfiguration
    {

        [Description("Provider invariant name")]
        public string ProviderInvariantName { get; set; }

        [Description("Connection String")]
        public string ConnectionString { get; set; }

        public Func<SqlManager, IQueryGenerator> QueryManager { get; set; }

        public Action<DbCommand, IQueryGenerator, StringBuilder> LogCommand { get; set; }

    }

}