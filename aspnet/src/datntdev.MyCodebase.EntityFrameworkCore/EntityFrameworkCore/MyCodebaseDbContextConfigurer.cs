using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace datntdev.MyCodebase.EntityFrameworkCore;

public static class MyCodebaseDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<MyCodebaseDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<MyCodebaseDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
