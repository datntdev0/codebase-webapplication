using Abp.Zero.EntityFrameworkCore;
using datntdev.MyCodebase.Authorization.Roles;
using datntdev.MyCodebase.Authorization.Users;
using datntdev.MyCodebase.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace datntdev.MyCodebase.EntityFrameworkCore;

public class MyCodebaseDbContext : AbpZeroDbContext<Tenant, Role, User, MyCodebaseDbContext>
{
    /* Define a DbSet for each entity of the application */

    public MyCodebaseDbContext(DbContextOptions<MyCodebaseDbContext> options)
        : base(options)
    {
    }
}
