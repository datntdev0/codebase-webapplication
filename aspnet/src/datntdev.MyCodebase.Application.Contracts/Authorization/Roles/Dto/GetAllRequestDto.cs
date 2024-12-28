using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace datntdev.MyCodebase.Authorization.Roles.Dto;

public class GetAllRequestDto : PagedResultRequestDto, IShouldNormalize
{
    public string Keyword { get; set; }
    public string Sorting { get; set; }
    public string Permission { get; set; }

    public void Normalize()
    {
        if (string.IsNullOrEmpty(Sorting))
        {
            Sorting = "Name,DisplayName";
        }

        Keyword = Keyword?.Trim();
        Permission = Permission?.Trim();
    }
}

