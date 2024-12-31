using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Authorization.Users.Dto;

public class ResetUserPasswordDto
{
    [Required]
    public string AdminPassword { get; set; }

    [Required]
    public long UserId { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
