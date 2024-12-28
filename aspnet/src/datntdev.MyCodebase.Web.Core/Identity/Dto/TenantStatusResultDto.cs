namespace datntdev.MyCodebase.Identity.Dto;

public class TenantStatusResultDto
{
    public TenantStatus Status { get; set; }

    public int? TenantId { get; set; }

    public TenantStatusResultDto(TenantStatus state, int? tenantId = null)
    {
        Status = state;
        TenantId = tenantId;
    }
}

public enum TenantStatus
{
    NotFound = 0,
    Available = 1,
    InActive = 2,
}
