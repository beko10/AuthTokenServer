namespace AuthTokenServer.EntityLayer.DTOs;

public class AddProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string UserId { get; set; } = null!;
}
