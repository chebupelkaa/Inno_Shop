namespace UserService.Application.Exceptions
{
    public class PasswordValidationException : ApplicationException
    {
        public PasswordValidationException(string message= "Confirmation password does not match with a new password") : base(message)
        {
        }
    }
}
