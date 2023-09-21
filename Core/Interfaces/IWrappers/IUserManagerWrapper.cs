using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Interfaces.IWrappers;
public interface IUserManagerWrapper
{
    Task<AppUser> GetUserByClaimsPrinciple(ClaimsPrincipal user);
    Task<AppUser> GetUserByClaimsPrincipleWithAddress(ClaimsPrincipal user);
    Task<AppUser> FindByEmailAsync(string email);
    Task<IdentityResult> UpdateAsync(AppUser user);
    Task<IdentityResult> CreateAsync(AppUser user, string password);
}
