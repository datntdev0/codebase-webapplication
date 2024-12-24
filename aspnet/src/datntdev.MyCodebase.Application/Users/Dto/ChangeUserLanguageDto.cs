using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}