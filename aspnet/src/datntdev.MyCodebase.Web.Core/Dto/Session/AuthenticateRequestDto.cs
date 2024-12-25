﻿using Abp.Auditing;
using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Models.Session
{
    public class AuthenticateRequestDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberClient { get; set; }
    }
}