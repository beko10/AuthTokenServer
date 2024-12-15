namespace AuthTokenServer.CoreLayer.Configuration;

/// <summary>
/// appsettings.json dosyasındaki Token yapılandırma ayarlarını programatik olarak tutan sınıf 
/// </summary>
public class CustomTokenOption
{
    public List<string> Audience { get; set; } = new List<string>(); // "Audience" alanı
    public string Issure { get; set; } = null!;                      // "Issure" alanı
    public int AccessTokenExpiration { get; set; }                  // "AccessTokenExpiration" alanı
    public int RefreshTokenExpiration { get; set; }                 // "RefreshTokenExpiration" alanı
    public string SecurityKey { get; set; } = null!;                // "SecurityKey" alanı
}
