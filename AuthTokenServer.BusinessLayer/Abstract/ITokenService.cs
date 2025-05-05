using AuthTokenServer.CoreLayer.Configuration;
using AuthTokenServer.EntityLayer.DTOs;
using AuthTokenServer.EntityLayer.Entities;

namespace AuthTokenServer.BusinessLayer.Abstract;

public interface ITokenService
{
    Task<TokenDto> CreateTokenAsync(AppUser user);
    ClientTokenDto CreateClientToken(Client client);
}
