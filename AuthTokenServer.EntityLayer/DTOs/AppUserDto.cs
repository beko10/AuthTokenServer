﻿namespace AuthTokenServer.EntityLayer.DTOs;

public class AppUserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
