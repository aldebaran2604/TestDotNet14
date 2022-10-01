
using TestApi.Interfaces;

namespace TestApi.OtherClasses;

public class ResponseInformation : IResponseInformation
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public bool Failure => !Success;
}