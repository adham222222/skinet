using System.Security.Authentication;
using System.Security.Claims;
using CORE.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static async Task<AppUser> GetUserByEmail(this UserManager<AppUser> userManager,
        ClaimsPrincipal user)
    {
        var User = await  userManager.Users.
              FirstOrDefaultAsync(x => x.Email == user.GetEmail()) ?? throw new AuthenticationException("User was not found");
        return User;
    }

    public static string GetEmail(this ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email)
             ?? throw new AuthenticationException("Email claim not found");
        return email;
    }

     public static async Task<AppUser> GetUserByEmailWithAddress(this UserManager<AppUser> userManager,
        ClaimsPrincipal user)
    {
        var User = await  userManager.Users.Include(x=>x.Address)
        .FirstOrDefaultAsync(x => x.Email == user.GetEmail()) ?? throw new AuthenticationException("User was not found");
        return User;
    }

}
