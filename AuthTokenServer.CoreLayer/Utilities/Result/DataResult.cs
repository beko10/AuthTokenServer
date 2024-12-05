namespace AuthTokenServer.CoreLayer.Utilities.Result;

public class DataResult<T> : Result, IDataResult<T>
{
    public T Data { get; }

    public string Message { get; }

    public bool IsSuccess { get; }

    public DataResult(T data, bool isSuccess, string message) : base(isSuccess, message)
    {
        Data = data;
    }
    public DataResult(T data, bool isSuccess) : base(isSuccess, string.Empty)
    {
        Data = data;
    }
}
