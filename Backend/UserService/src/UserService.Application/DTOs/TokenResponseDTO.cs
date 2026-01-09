namespace UserService.Application.DTOs
{
    public class TokenResponseDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public UserDTO User { get; set; } = null!;

    }
}
//public string Token { get; set; } = string.Empty;
//public DateTime Expires { get; set; }