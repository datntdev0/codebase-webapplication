using Abp.Application.Services;
using datntdev.MyCodebase.Sessions.Dto;
using System.Threading.Tasks;

namespace datntdev.MyCodebase.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
