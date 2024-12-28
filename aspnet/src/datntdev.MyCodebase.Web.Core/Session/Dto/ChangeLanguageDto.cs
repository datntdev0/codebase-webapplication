using System.ComponentModel.DataAnnotations;

namespace datntdev.MyCodebase.Session.Dto;

public class ChangeLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}