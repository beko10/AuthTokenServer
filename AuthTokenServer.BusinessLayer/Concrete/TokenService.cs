using AuthTokenServer.BusinessLayer.Abstract; // İş katmanı için kullanılan soyut servislerin referansı.
using AuthTokenServer.BusinessLayer.Concrete; // İş katmanı için somut servislerin referansı.
using AuthTokenServer.CoreLayer.Configuration; // Core katmanındaki token ayarları için gerekli yapılandırma sınıfı.
using AuthTokenServer.EntityLayer.DTOs; // Token oluştururken kullanılan DTO sınıfları.
using AuthTokenServer.EntityLayer.Entities; // Kullanıcı ve client gibi uygulama varlıklarının referansı.
using Microsoft.AspNetCore.Identity; // Microsoft Identity Framework için gerekli kütüphane.
using Microsoft.Extensions.Options; // Dependency Injection ile ayar sınıflarını almak için kullanılan yapı.
using Microsoft.IdentityModel.Tokens; // JWT oluşturmak ve doğrulamak için gerekli kütüphane.
using System.IdentityModel.Tokens.Jwt; // JWT token oluşturma ve işleme sınıfı.
using System.Security.Claims; // Kullanıcı veya client bilgilerini tutan claim yapısını sağlamak için kullanılan kütüphane.
using System.Security.Cryptography; // Güvenli rastgele sayı üretimi için kullanılan kütüphane.

public class TokenService : ITokenService // Token servisinin ITokenService arayüzünü implemente eden sınıf.
{
    // Identity kullanıcı yönetimi için UserManager instance'ı
    private readonly UserManager<AppUser> _userManager;

    // Token ayarlarını tutan options sınıfı instance'ı
    private readonly CustomTokenOption _customTokenOption;

    // Constructor - Dependency injection ile gereken servisleri alır
    public TokenService(UserManager<AppUser> userManager, IOptions<CustomTokenOption> options)
    {
        _userManager = userManager; // Kullanıcı yönetim işlemleri için UserManager atanır.
        _customTokenOption = options.Value; // Custom token ayarları options'tan alınır.
    }

    // Refresh token oluşturan private metot
    private string CreateRefreshToken()
    {
        var numberByte = new Byte[32]; // 32 byte'lık boş bir dizi oluşturulur.
        using var rnd = RandomNumberGenerator.Create(); // Güvenli bir rastgele sayı üretici oluşturulur.
        rnd.GetBytes(numberByte); // Byte dizisini rastgele değerlerle doldurur.
        return Convert.ToBase64String(numberByte); // Rastgele byte dizisini base64 string'e çevirip döndürür.
    }

    // Kullanıcı için JWT claim'lerini oluşturan private metot
    private async Task<IEnumerable<Claim>> GetClaim(AppUser user, List<string> audinces)
    {

        var userRoles = await _userManager.GetRolesAsync(user); // Kullanıcının rollerini alır.
        // Kullanıcıya ait claim bilgilerini liste olarak oluşturur.
        var userList = new List<Claim>()
       {
           new Claim(ClaimTypes.NameIdentifier,user.Id), // Kullanıcının benzersiz ID bilgisi
           new Claim(JwtRegisteredClaimNames.Email,user.Email), // Kullanıcı e-posta bilgisi
           new Claim(ClaimTypes.Name,user.UserName), // Kullanıcı adı bilgisi
           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()) // JWT'nin benzersiz ID'si
       };

        // Kullanıcının rollerini claim'lere ekler.
        userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

        // Gelen audience bilgilerini claim'lere ekler.
        userList.AddRange(audinces.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        // Kullanıcıya ait claim bilgilerini döndürür.
        return userList; 
    }

    // Client uygulaması için JWT claim'lerini oluşturan private metot
    private IEnumerable<Claim> GetClaimsByClient(Client client)
    {
        // Client ile ilişkili claim bilgilerini liste olarak oluşturur.
        var claims = new List<Claim>()
       {
           new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), // Benzersiz JWT ID'si
           new Claim(JwtRegisteredClaimNames.Sub, client.Id) // Client ID'si
       };

        // Client'a ait audience bilgilerini claim'lere ekler.
        claims.AddRange(client.Audience.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        return claims; // Claim listesi döndürülür.
    }

    // Client uygulaması için token oluşturan public metot
    public ClientTokenDto CreateClientToken(Client client)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration); // Token son kullanma tarihini hesaplar.
        var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey); // Simetrik güvenlik anahtarını oluşturur.
        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Token'ı imzalamak için kimlik bilgilerini oluşturur.

        // JWT token nesnesini oluşturur ve yapılandırır.
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOption.Issuer, // Token'ı oluşturan uygulama.
            signingCredentials: signingCredentials, // Token imza bilgisi.
            expires: accessTokenExpiration, // Token son kullanma tarihi.
            claims: GetClaimsByClient(client)); // Client claim bilgileri.

        var jwtHandler = new JwtSecurityTokenHandler(); // JWT handler oluşturulur.
        var token = jwtHandler.WriteToken(jwtSecurityToken); // Token'ı string formatına çevirir.

        // Oluşturulan token bilgilerini DTO'ya aktarır.
        ClientTokenDto clientTokenDto = new ClientTokenDto
        {
            AccessToken = token, // JWT access token.
            AccessTokenExpiration = accessTokenExpiration // Token son kullanma tarihi.
        };

        return clientTokenDto; // DTO nesnesini döndürür.
    }

    // Kullanıcı için token oluşturan public metot
    public async Task<TokenDto> CreateTokenAsync(AppUser user)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.AccessTokenExpiration); // Token son kullanma tarihini hesaplar.

        var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOption.RefreshTokenExpiration); // Refresh token son kullanma tarihi.

        var securityKey = SignService.GetSymmetricSecurityKey(_customTokenOption.SecurityKey); // Simetrik güvenlik anahtarını oluşturur.

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); // Token'ı imzalamak için kimlik bilgilerini oluşturur.

        // JWT token nesnesini oluşturur ve yapılandırır.
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _customTokenOption.Issuer, // Token'ı oluşturan uygulama.
            expires: accessTokenExpiration, // Token son kullanma tarihi.
            signingCredentials: signingCredentials, // Token imza bilgisi.
            notBefore: DateTime.Now, // Token'ın geçerli olmaya başladığı zaman.
            claims: await GetClaim(user, _customTokenOption.Audience)); // Kullanıcı claim bilgileri.

        var jwtHandler = new JwtSecurityTokenHandler(); // JWT handler oluşturulur.
        var token = jwtHandler.WriteToken(jwtSecurityToken); // Token'ı string formatına çevirir.

        // Oluşturulan token bilgilerini DTO'ya aktarır.
        var TokenDto = new TokenDto
        {
            AccessToken = token, // JWT access token.
            AccessTokenExpiration = accessTokenExpiration, // Token son kullanma tarihi.
            RefreshTokenExpiration = refreshTokenExpiration, // Refresh token son kullanma tarihi.
            RefreshToken = CreateRefreshToken() // Yeni bir refresh token oluşturur.
        };

        return TokenDto; // DTO nesnesini döndürür.
    }

}
