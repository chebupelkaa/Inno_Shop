using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Exceptions;

namespace ProductService.Application.Common
{
    public static class CurrentUser
    {
        public static int GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }

            return userId;
        }
    }
}
