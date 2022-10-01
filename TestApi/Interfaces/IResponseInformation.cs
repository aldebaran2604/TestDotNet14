namespace TestApi.Interfaces;

public interface IResponseInformation
{
    bool Success { get; set; }

    string? Message { get; set; }

    bool Failure { get; }
}