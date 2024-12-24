using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Configuration.Dto;

public class ChangeUiThemeInput
{
    [Required]
    [StringLength(32)]
    public string Theme { get; set; }
}
