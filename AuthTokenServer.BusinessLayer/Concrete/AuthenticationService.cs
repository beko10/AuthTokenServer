using AuthTokenServer.BusinessLayer.Abstract;
using AuthTokenServer.CoreLayer.Configuration;
using AuthTokenServer.CoreLayer.DataAccessLayer.Abstract;
using AuthTokenServer.CoreLayer.DataAccessLayer.UnitOfWork;
using AuthTokenServer.CoreLayer.Utilities.Result;
using AuthTokenServer.DataAccessLayer.Abstract;
using AuthTokenServer.EntityLayer.DTOs;
using AuthTokenServer.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthTokenServer.BusinessLayer.Concrete;

public class AuthenticationService : IAuthenticationService
{
    private readonly List<Client> _clients; // Client bilgilerini tutan liste.
    private readonly IUnitOfWork _unitOfWork; // Veritabanı işlemlerini yönetmek için UnitOfWork arayüzü.
    private readonly UserManager<AppUser> _userManager; // Kullanıcı işlemleri için Identity UserManager.
    private readonly IUserRefreshTokenRepositories _userRefreshTokenRepositories;  // UserRefreshToken tablosu için genel repository.
    private readonly ITokenService _tokenService; // Token oluşturma işlemleri için servis.

    public AuthenticationService(IOptions<List<Client>> clients, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ITokenService tokenService, IUserRefreshTokenRepositories userRefreshTokenRepositories)
    {
        _clients = clients.Value; // Dependency Injection ile gelen client bilgilerini ayarla.
        _unitOfWork = unitOfWork; // Dependency Injection ile gelen UnitOfWork nesnesini ayarla.
        _userManager = userManager; // Dependency Injection ile gelen UserManager nesnesini ayarla.
        _tokenService = tokenService; // Dependency Injection ile gelen TokenService nesnesini ayarla.
        _userRefreshTokenRepositories = userRefreshTokenRepositories;// Dependency Injection ile gelen repository nesnesini ayarla.
    }

    public async Task<IDataResult<TokenDto>> CreateTokenAsync(LoginDto loginDto)
    {
        if (loginDto == null) throw new ArgumentNullException(nameof(loginDto)); // Eğer loginDto null ise hata fırlat.

        var user = await _userManager.FindByEmailAsync(loginDto.Email); // Kullanıcıyı email adresine göre bul.

        if (user == null) return new ErrorDataResult<TokenDto>("Email or Password wrong..."); // Kullanıcı bulunamazsa hata dön.

        if (!await _userManager.CheckPasswordAsync(user, loginDto.Password)) // Kullanıcının şifresi yanlışsa hata dön.
        {
            return new ErrorDataResult<TokenDto>("Email or Password wrong...");
        }

        var token = await _tokenService.CreateTokenAsync(user); // Kullanıcı için bir token oluştur.

        var userRefreshToken = await _userRefreshTokenRepositories.Where(x => x.UserId == user.Id).SingleOrDefaultAsync(); // Kullanıcının refresh token'ını bul.

        if (userRefreshToken == null) // Eğer refresh token yoksa yeni bir refresh token oluştur.
        {
            var newUserRefreshToken = new UserRefreshToken
            {
                UserId = user.Id,
                RefreshToken =  token.RefreshToken,
                Expiration =  token.RefreshTokenExpiration
            };
            await _userRefreshTokenRepositories.CreateAsync(newUserRefreshToken); // Yeni refresh token'ı veritabanına kaydet.
        }
        else // Eğer refresh token varsa, mevcut olanı güncelle.
        {
            userRefreshToken.Expiration =  token.RefreshTokenExpiration;
            userRefreshToken.RefreshToken =  token.RefreshToken;
        }

        await _unitOfWork.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet.

        return new SuccessDataResult<TokenDto>(token, "Token created successfully..."); // Oluşturulan token ile başarılı sonucu dön.
    }

    public IDataResult<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret); // Client bilgilerini doğrula.

        if (client == null) return new ErrorDataResult<ClientTokenDto>("Client is not found..."); // Client bulunamazsa hata dön.

        var token = _tokenService.CreateClientToken(client); // Client için bir token oluştur.

        return new SuccessDataResult<ClientTokenDto>(token, "Token created successfully..."); // Oluşturulan token ile başarılı sonucu dön.
    }

    public async Task<IDataResult<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var hasRefreshToken = await _userRefreshTokenRepositories.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync(); // Refresh token'ı bul.

        if (hasRefreshToken == null) // Eğer refresh token bulunamazsa hata dön.
        {
            return new ErrorDataResult<TokenDto>("Refresh token not found...");
        }

        var user = await _userManager.FindByIdAsync(hasRefreshToken.UserId); // Refresh token'a bağlı kullanıcıyı bul.

        if (user == null) // Kullanıcı bulunamazsa hata dön.
        {
            return new ErrorDataResult<TokenDto>("User not found...");
        }

        var tokenDto = await _tokenService.CreateTokenAsync(user); // Kullanıcı için yeni bir token oluştur.

        hasRefreshToken.RefreshToken = tokenDto.RefreshToken; // Refresh token'ı güncelle.
        hasRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

        await _unitOfWork.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet.

        return new SuccessDataResult<TokenDto>(tokenDto, "Token created successfully..."); // Yeni token ile başarılı sonucu dön.
    }

    public async Task<IResult> RevokeRefreshToken(string refreshToken)
    {
        var hasRefreshToken = await _userRefreshTokenRepositories.Where(x => x.RefreshToken == refreshToken).SingleOrDefaultAsync(); // Refresh token'ı bul.

        if (hasRefreshToken == null) // Eğer refresh token bulunamazsa hata dön.
        {
            return new ErrorResult("Refresh token not found...");
        }

        _userRefreshTokenRepositories.Delete(hasRefreshToken); // Refresh token'ı sil.
        await _unitOfWork.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet.
        return new SuccessDataResult<TokenDto>("Refresh token deleted successfully..."); // Başarı mesajı dön.
    }
}
