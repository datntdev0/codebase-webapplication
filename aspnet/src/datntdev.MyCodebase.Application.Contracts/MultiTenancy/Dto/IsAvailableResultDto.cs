namespace datntdev.MyCodebase.MultiTenancy.Dto;

public class IsAvailableResultDto
{
    public AvailabilityState State { get; set; }

    public int? TenantId { get; set; }

    public IsAvailableResultDto()
    {
    }

    public IsAvailableResultDto(AvailabilityState state, int? tenantId = null)
    {
        State = state;
        TenantId = tenantId;
    }
}
