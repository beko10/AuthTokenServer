﻿namespace AuthTokenServer.EntityLayer.DTOs;

public class ClientLoginDto
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
