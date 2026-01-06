namespace School_DTOs
{
    public class APIException
    {
        public string Message { get; set; } = string.Empty;
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }

        public APIException(string message, Exception? innerException = null)
        {
            Message = message;
            InnerException = innerException?.Message;
            StackTrace = innerException?.StackTrace;
        }
    }
}

