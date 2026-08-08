using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Application.Features.Auth.Commands.ConfirmEmail;
using UserService.Application.Features.Auth.Commands.ForgotPassword;
using UserService.Application.Features.Auth.Commands.Login;
using UserService.Application.Features.Auth.Commands.RefreshToken;
using UserService.Application.Features.Auth.Commands.Register;
using UserService.Application.Features.Auth.Commands.ResetPassword;
using UserService.Application.Features.Auth.Commands.SendEmailConfirmation;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            await _mediator.Send(command);
            return Ok(new
            {
                message = "Registration successful. Please confirm your email before logging in."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token");

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new
            {
                message = "If the email address exists in our system, you will receive a password reset link."
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { Message = "Password has been successfully reset. Please login with your new password" });
        }

        [HttpPost("send-email-confirmation")]
        public async Task<IActionResult> SendEmailConfirmation(SendEmailConfirmationCommand command)
        {
            await _mediator.Send(command);
            return Ok(new { message = "If the account exists and is not confirmed, a confirmation email has been sent." });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            await _mediator.Send(new ConfirmEmailCommand { Email = email, Token = token });
            return Ok(new { message = "Email has been successfully confirmed. You can now log in." });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Role = role,
                IsAuthenticated = User.Identity?.IsAuthenticated
            });
        }
    }
}
