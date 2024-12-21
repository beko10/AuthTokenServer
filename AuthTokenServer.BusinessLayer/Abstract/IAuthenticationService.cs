using AuthTokenServer.CoreLayer.Configuration;
using AuthTokenServer.CoreLayer.Utilities.Result;
using AuthTokenServer.EntityLayer.DTOs;

namespace AuthTokenServer.BusinessLayer.Abstract;

public interface IAuthenticationService
{
    Task<IDataResult<TokenDto>> CreateTokenAsync(LoginDto loginDto);

    Task<IDataResult<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
    
    Task<IResult> RevokeRefreshToken(string refreshToken);

    IDataResult<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);


}
