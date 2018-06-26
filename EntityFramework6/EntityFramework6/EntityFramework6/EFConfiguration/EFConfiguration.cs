using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace EntityFramework6.EFConfiguration
{
    public class EFConfiguration : DbConfiguration
    {
        public EFConfiguration()
        {

            SetDatabaseInitializer(new NullDatabaseInitializer<EfDbContext>());

            SetManifestTokenResolver(new ManifestTokenResolver());

        }
    }

    public class ManifestTokenResolver : IManifestTokenResolver
    {
        private readonly IManifestTokenResolver _defaultResolver = new DefaultManifestTokenResolver();

        public string ResolveManifestToken(DbConnection connection)
        {
            if (connection is SqlConnection sqlConn)
            {
                return "2012";
            }
            else
            {
                return _defaultResolver.ResolveManifestToken(connection);
            }
        }
    }
}

