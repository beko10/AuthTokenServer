using Microsoft.AspNetCore.Identity;

namespace AuthTokenServer.EntityLayer.Entities;

public class AppUser:IdentityUser
{
    public string City { get; set; } = null!;   
}
