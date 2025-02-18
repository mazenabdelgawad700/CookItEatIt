using System.Net;
namespace RecipeApp.Shared.Bases;

public class ReturnBase<T>
{
    public ReturnBase() { }
    public ReturnBase(T data, string? message = null)
    {
        Succeeded = true;
        Message = message!;
        Data = data;
    }
    public ReturnBase(string message)
    {
        Succeeded = false;
        Message = message;
    }
    public ReturnBase(string message, bool succeeded)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public HttpStatusCode StatusCode { get; set; }
    public object? Meta { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public object? Data { get; set; }
}