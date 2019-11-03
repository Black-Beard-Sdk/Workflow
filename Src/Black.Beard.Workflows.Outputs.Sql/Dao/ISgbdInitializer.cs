using System.Collections.Generic;

namespace Bb.Dao
{

    public interface ISgbdInitializer
    {

        /// <summary>
        /// Registers the connexion and initialize dao.
        /// </summary>
        /// <param name="providerInvariantName">Name of the provider invariant.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        SqlManagerConfiguration RegisterConnexion(string providerInvariantName, string connectionString);

        void InitializeDatabase(SqlManagerConfiguration configuration, ISgbdUpdater initializer, int targetVersion, Dictionary<string, string> arguments);

    }
}