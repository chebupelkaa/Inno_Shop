namespace UserService.Application.Exceptions
{
    public class BaseException : ApplicationException
    {
        public BaseException(string message) : base(message)    { }
    }
}
