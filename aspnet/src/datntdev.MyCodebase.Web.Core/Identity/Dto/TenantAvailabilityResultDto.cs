namespace datntdev.MyCodebase.Identity.Dto;

public class TenantAvailabilityResultDto
{
    public AvailabilityState State { get; set; }

    public int? TenantId { get; set; }

    public TenantAvailabilityResultDto()
    {
    }

    public TenantAvailabilityResultDto(AvailabilityState state, int? tenantId = null)
    {
        State = state;
        TenantId = tenantId;
    }
}

public enum AvailabilityState
{
    Available = 1,
    InActive,
    NotFound
}
