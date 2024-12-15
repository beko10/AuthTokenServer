namespace AuthTokenServer.CoreLayer.Configuration;

public class Client
{
    public string Id { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public ICollection<string> Audience { get; set; } = [];
}
