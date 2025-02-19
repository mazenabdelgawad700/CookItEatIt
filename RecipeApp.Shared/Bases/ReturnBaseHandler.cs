using System.Net;

namespace RecipeApp.Shared.Bases;
public static class ReturnBaseHandler
{
    #region Handle Functions
    public static ReturnBase<T> Success<T>(T entity, string? message)
    {
        return new ReturnBase<T>()
        {
            Data = entity,
            StatusCode = HttpStatusCode.OK,
            Succeeded = true,
            Message = message ?? "Success",
        };
    }
    public static ReturnBase<T> Unauthorized<T>(string message = null!)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Succeeded = true,
            Message = message ?? "Unauthorized"
        };
    }
    public static ReturnBase<T> LoginTimeOut<T>(string message = null!)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.Forbidden,
            Succeeded = true,
            Message = message ?? "Login Time Out"
        };
    }
    public static ReturnBase<T> Failed<T>(string? message = null)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.ExpectationFailed,
            Succeeded = false,
            Message = message ?? "Failed"
        };
    }
    public static ReturnBase<T> BadRequest<T>(string message = null!)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = message ?? "Bad Request"
        };
    }
    public static ReturnBase<T> UnprocessableEntity<T>(string message = null!)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.UnprocessableEntity,
            Succeeded = false,
            Message = message ?? "Unprocessable Entity"
        };
    }
    public static ReturnBase<T> NotFound<T>(string message = null!)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.NotFound,
            Succeeded = false,
            Message = message ?? "NotFound"
        };
    }
    public static ReturnBase<T> Created<T>(T entity, string? message)
    {
        return new ReturnBase<T>()
        {
            Data = entity,
            StatusCode = HttpStatusCode.Created,
            Succeeded = true,
            Message = message ?? "Created",
        };
    }
    public static ReturnBase<T> Updated<T>(string? message)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.OK,
            Succeeded = true,
            Message = message ?? "Updated"
        };
    }
    public static ReturnBase<T> Deleted<T>(string? message)
    {
        return new ReturnBase<T>()
        {
            StatusCode = HttpStatusCode.OK,
            Succeeded = true,
            Message = message ?? "Deleted"
        };
    }
    #endregion
}

