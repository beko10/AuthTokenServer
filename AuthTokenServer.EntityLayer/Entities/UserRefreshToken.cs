namespace AuthTokenServer.EntityLayer.Entities;

public class UserRefreshToken
{
    public string UserId { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime Expiration { get; set; }
}
