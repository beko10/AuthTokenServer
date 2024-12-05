using AuthTokenServer.CoreLayer.EntityLayer;

namespace AuthTokenServer.EntityLayer.Entities;

public class Product:BaseEntity
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string UserId { get; set; } = null!;


}
