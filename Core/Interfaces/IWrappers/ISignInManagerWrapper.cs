using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces.IWrappers;
public interface ISignInManagerWrapper
{
    Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password, bool isPersistent);
}
