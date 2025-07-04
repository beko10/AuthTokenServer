﻿using AuthTokenServer.CoreLayer.Utilities.Result;
using AuthTokenServer.EntityLayer.DTOs;

namespace AuthTokenServer.BusinessLayer.Abstract;

public interface IUserService
{
    Task<IDataResult<AppUserDto>> CreateUserAsync(CreateUserDto user);
    Task<IDataResult<AppUserDto>> GetUserByUserNameAsync(string userName);
    Task<Result> CreateUserRoles(string id);
}
