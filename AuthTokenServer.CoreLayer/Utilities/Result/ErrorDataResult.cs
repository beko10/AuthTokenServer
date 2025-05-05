namespace AuthTokenServer.CoreLayer.Utilities.Result;

public class ErrorDataResult<T> : DataResult<T>
{
    public List<string> Errors { get; }
    public ErrorDataResult(T data, List<string> message) : base(data, false)
    {
        Errors = message;
    }
    public ErrorDataResult(T data, string message) : base(data, false, message)
    {
    }

    public ErrorDataResult(T data) : base(data, false)
    {
    }
    public ErrorDataResult(string message) : base(default, false, message)
    {
    }

    public ErrorDataResult() : base(default, false)
    {
    }
}
