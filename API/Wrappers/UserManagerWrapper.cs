using API.Extenstions;
using Core.Entities.Identity;
using Core.Interfaces.IWrappers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Wrappers;

public class UserManagerWrapper : IUserManagerWrapper
{
    private readonly UserManager<AppUser> _userManager;

    public UserManagerWrapper(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<AppUser> GetUserByClaimsPrinciple(ClaimsPrincipal user)
    {
        return await _userManager.GetUserByClaimsPrinciple(user);
    }

    public async Task<AppUser> GetUserByClaimsPrincipleWithAddress(ClaimsPrincipal user)    
    {
        return await _userManager.GetUserByClaimsPrincipleWithAddress(user);
    }

    public async Task<AppUser> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> UpdateAsync(AppUser user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> CreateAsync(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }
}
