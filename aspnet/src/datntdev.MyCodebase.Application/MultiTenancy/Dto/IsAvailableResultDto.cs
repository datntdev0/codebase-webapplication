using datntdev.MyCodebase.Authorization.Accounts.Dto;

namespace datntdev.MyCodebase.MultiTenancy.Dto;

public class IsAvailableResultDto
{
    public TenantAvailabilityState State { get; set; }

    public int? TenantId { get; set; }

    public IsAvailableResultDto()
    {
    }

    public IsAvailableResultDto(TenantAvailabilityState state, int? tenantId = null)
    {
        State = state;
        TenantId = tenantId;
    }
}
