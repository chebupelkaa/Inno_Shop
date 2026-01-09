namespace UserService.Application.Exceptions
{
    public class TokenAlreadyRequestedException : ApplicationException
    {
        public DateTime? ExpiryTime { get; }
        public int MinutesLeft { get; }

        public TokenAlreadyRequestedException(DateTime expiryTime)
            : base($"Password reset already requested. Please check your email or wait {(int)(expiryTime - DateTime.UtcNow).TotalMinutes} minutes.")
        {
            ExpiryTime = expiryTime;
            MinutesLeft = (int)(expiryTime - DateTime.UtcNow).TotalMinutes;
        }
    }
}
