using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Authorization.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}