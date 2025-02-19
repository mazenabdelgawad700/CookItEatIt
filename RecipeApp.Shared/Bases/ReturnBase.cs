using System.Net;
namespace RecipeApp.Shared.Bases;

public class ReturnBase<T>
{
    public ReturnBase()
    {
        Succeeded = true;
        Errors = new List<string>();
        Message = "";
    }
    public ReturnBase(T data, string? message = null)
    {
        Succeeded = true;
        Message = message!;
        Data = data;
        Errors = new List<string>();
    }
    public ReturnBase(T data, string message, bool succeeded)
    {
        Succeeded = succeeded;
        Message = message;
        Errors = new List<string>();
        Data = data;
    }

    public HttpStatusCode StatusCode { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; }
    public T Data { get; set; }
}