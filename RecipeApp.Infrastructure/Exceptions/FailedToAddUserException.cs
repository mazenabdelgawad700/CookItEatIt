namespace RecipeApp.Infrastructure.Exceptions
{
    public class FailedToAddUserException : Exception
    {
        public int ErrorCode { get; set; }

        public FailedToAddUserException() : base() { }

        public FailedToAddUserException(string message) : base(message) { }

        public FailedToAddUserException(string message, Exception innerException)
            : base(message, innerException) { }

        public FailedToAddUserException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
