namespace TestApi.Interfaces;

public interface IResponseInformation<T> : IResponseInformation
{
    T? ResultItem { get; set; }
}