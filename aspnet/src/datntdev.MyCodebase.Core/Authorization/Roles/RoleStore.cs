using Abp.Authorization.Roles;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using datntdev.MyCodebase.Authorization.Users;

namespace datntdev.MyCodebase.Authorization.Roles;

public class RoleStore(
    IUnitOfWorkManager unitOfWorkManager,
    IRepository<Role> roleRepository,
    IRepository<RolePermissionSetting, long> rolePermissionSettingRepository
) : AbpRoleStore<Role, User>(
    unitOfWorkManager,
    roleRepository,
    rolePermissionSettingRepository)
{ }
