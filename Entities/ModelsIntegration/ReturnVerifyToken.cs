namespace Entities.ModelsIntegration;

public class ReturnVerifyToken
{
    public string kind { get; set; }
    public string idToken { get; set; }
    public string refreshToken { get; set; }
    public string expiresIn { get; set; }
    public bool isNewUser { get; set; }
}
