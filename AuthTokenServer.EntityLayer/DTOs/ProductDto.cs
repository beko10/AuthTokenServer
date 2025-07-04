﻿namespace AuthTokenServer.EntityLayer.DTOs;

public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string UserId { get; set; } = null!;
}
