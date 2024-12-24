using Abp.Application.Services;
using datntdev.MyCodebase.Authorization.Accounts.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Authorization.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

    Task<RegisterOutput> Register(RegisterInput input);
}
