using System.Security.Claims;

namespace ShowCase.Services.Account;

public static class ClaimsPrincipalExtensions
{
    public static int Id(this ClaimsPrincipal user)
    {
        if (int.TryParse(user.FindFirstValue("Id"), out int id))
        {
            return id;
        }

        return 0;
    }
}