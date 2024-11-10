using System.Security.Claims;

namespace ChatkaReservation.Extensions
{
    public static class UserExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            // Retrieve the user ID from claims (assuming it's stored as an integer)
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }
    }
}
