﻿using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace datntdev.MyCodebase.Authorization.Users.Dto;

public class GetAllUsersDto : PagedResultRequestDto, IShouldNormalize
{
    public string Keyword { get; set; }
    public bool? IsActive { get; set; }

    public string Sorting { get; set; }

    public void Normalize()
    {
        if (string.IsNullOrEmpty(Sorting))
        {
            Sorting = "UserName,EmailAddress";
        }

        Keyword = Keyword?.Trim();
    }
}
