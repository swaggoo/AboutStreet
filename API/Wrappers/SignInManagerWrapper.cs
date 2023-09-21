using Core.Entities.Identity;
using Core.Interfaces.IWrappers;
using Microsoft.AspNetCore.Identity;

namespace API.Wrappers;

public class SignInManagerWrapper : ISignInManagerWrapper
{
    private readonly SignInManager<AppUser> _signInManager;

    public SignInManagerWrapper(SignInManager<AppUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password, bool isPersistent)
    {
        return await _signInManager.CheckPasswordSignInAsync(user, password, isPersistent);
    }
}
