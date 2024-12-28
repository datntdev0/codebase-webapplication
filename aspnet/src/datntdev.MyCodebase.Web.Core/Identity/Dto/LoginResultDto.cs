namespace datntdev.MyCodebase.Identity.Dto;

public class LoginResultDto
{
    public string AccessToken { get; set; }

    public string EncryptedAccessToken { get; set; }

    public int ExpireInSeconds { get; set; }

    public long UserId { get; set; }
}
