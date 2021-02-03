using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.EntityFrameworkCore
{
    public static class FirstProjectDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<FirstProjectDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<FirstProjectDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
