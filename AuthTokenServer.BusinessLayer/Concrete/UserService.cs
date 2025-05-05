using AuthTokenServer.BusinessLayer.Abstract;
using AuthTokenServer.CoreLayer.Utilities.Result;
using AuthTokenServer.EntityLayer.DTOs;
using AuthTokenServer.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthTokenServer.BusinessLayer.Concrete;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IDataResult<AppUserDto>> CreateUserAsync(CreateUserDto user)
    {
        var createdUser = new AppUser
        {
            UserName = user.UserName,
            Email = user.Email,
        };

        var result = await _userManager.CreateAsync(createdUser, user.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return new ErrorDataResult<AppUserDto>(null,errors);
        }
        return new SuccessDataResult<AppUserDto>(new AppUserDto
        {
            Id = createdUser.Id,
            UserName = createdUser.UserName,
            Email = createdUser.Email,
            City = createdUser.City,
        });
    }

    public async Task<IDataResult<AppUserDto>> GetUserByUserNameAsync(string userName)
    {
       var hasUser = await _userManager.FindByNameAsync(userName);
       if(hasUser is null)
       {
            return new  ErrorDataResult<AppUserDto>(null,"User not found");
       }

        return new SuccessDataResult<AppUserDto>(new AppUserDto
        {
            Id = hasUser.Id,
            UserName = hasUser.UserName,
            Email = hasUser.Email,
            City = hasUser.City
        }, "User is Found");
    }

    public async Task<Result> CreateUserRoles(string id)
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new() { Name = "Admin" });
            await _roleManager.CreateAsync(new() { Name = "Manager" });
            await _roleManager.CreateAsync(new() { Name = "User" });
        }

        var hasUser = await _userManager.FindByIdAsync(id);
        
        if (hasUser is null)
        {
            return new ErrorResult("User not found");
        }

        await _userManager.AddToRoleAsync(hasUser, "Admin");
        await _userManager.AddToRoleAsync(hasUser, "User");
        await _userManager.AddToRoleAsync(hasUser, "Manager");

        return new SuccessResult("User roles created");


    }

}
