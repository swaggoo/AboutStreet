using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extenstions;

public static class UserManagerExtensions
{
    public static async Task<AppUser> GetUserByClaimsPrincipleWithAddress(
        this UserManager<AppUser> userManager, 
        ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        return await userManager.Users.Include(x => x.Address)
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public static async Task<AppUser> GetUserByClaimsPrinciple(this UserManager<AppUser> userManager,
        ClaimsPrincipal user)
    {
        var email = user.FindFirstValue(ClaimTypes.Email);

        return await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}
