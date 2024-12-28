using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Session.Dto;

public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
