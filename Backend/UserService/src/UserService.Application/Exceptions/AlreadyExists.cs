namespace UserService.Application.Exceptions
{
    public class AlreadyExists : ApplicationException
    {
        public AlreadyExists(string message) : base(message) { }
    }
}
