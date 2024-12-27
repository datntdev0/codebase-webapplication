using Abp.Application.Services;
using datntdev.MyCodebase.Identity.Accounts.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Identity.Accounts;

public interface IAccountAppService : IApplicationService
{
    Task<RegisterResultDto> Register(RegisterRequestDto input);
}
