﻿namespace AuthTokenServer.EntityLayer.DTOs;

public class ClientTokenDto
{
    public string AccessToken { get; set; } = null!;
    public DateTime AccessTokenExpiration { get; set; }
}
