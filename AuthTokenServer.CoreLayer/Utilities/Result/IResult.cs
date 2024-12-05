namespace AuthTokenServer.CoreLayer.Utilities.Result;

public interface IResult
{
    public string Message { get;} 
    public bool IsSuccess { get;}
}
