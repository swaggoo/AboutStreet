using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.IWrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly IUserManagerWrapper _userManagerWrapper;
    private readonly ISignInManagerWrapper _signInManagerWrapper;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(
        IUserManagerWrapper userManagerWrapper,
        ISignInManagerWrapper signInManagerWrapper,
        ITokenService tokenService,
        IMapper mapper)
    {
        _userManagerWrapper = userManagerWrapper;
        _signInManagerWrapper = signInManagerWrapper;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await _userManagerWrapper.GetUserByClaimsPrinciple(User);

        return new UserDto
        { 
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    [HttpGet("emailExists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync(string email)
    {
        return await _userManagerWrapper.FindByEmailAsync(email) != null;
    }

    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
        var user = await _userManagerWrapper.GetUserByClaimsPrincipleWithAddress(User);

        return _mapper.Map<Address, AddressDto>(user.Address);
    }

    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var user = await _userManagerWrapper.GetUserByClaimsPrincipleWithAddress(User);

        user.Address = _mapper.Map<AddressDto, Address>(address);

        var result = await _userManagerWrapper.UpdateAsync(user);

        if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

        return BadRequest("Problem updating the user");
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManagerWrapper.FindByEmailAsync(loginDto.Email);

        if (user == null) return Unauthorized(new ApiResponse(401));

        var result = await _signInManagerWrapper.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        var token = _tokenService.CreateToken(user);

        return new UserDto
        {
            Email = user.Email,
            Token = token,
            DisplayName = user.DisplayName
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
        {
            return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
        }

        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManagerWrapper.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(new ApiResponse(400));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Email = user.Email
        };
    }
}
