using Abp.MultiTenancy;
using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.MultiTenancy.Dto;

public class IsAvailableRequestDto
{
    [Required]
    [StringLength(AbpTenantBase.MaxTenancyNameLength)]
    public string TenancyName { get; set; }
}
