using TestApi.Interfaces;

namespace TestApi.OtherClasses;

public class ResponseInformation<T> : ResponseInformation, IResponseInformation<T>
{
    public T? ResultItem { get; set; }
}