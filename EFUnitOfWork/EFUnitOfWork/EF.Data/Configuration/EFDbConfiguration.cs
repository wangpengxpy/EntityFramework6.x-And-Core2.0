using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;

namespace EF.Data.Configuration
{
    public class EFDbConfiguration : DbConfiguration
    {
        public EFDbConfiguration()
        {
            //数据库初始化
            SetDatabaseInitializer(new NullDatabaseInitializer<EFDbContext>());

            //指定数据库版本
            SetManifestTokenResolver(new ManifestTokenResolver());

            //设置模型缓存
            SetModelStore(new DefaultDbModelStore(Directory.GetCurrentDirectory()));
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
