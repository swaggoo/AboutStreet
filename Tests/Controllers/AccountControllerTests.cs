using API.Controllers;
using API.DTOs;
using API.Errors;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.IWrappers;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Tests.Controllers;
public class AccountControllerTests
{
    private readonly Mock<IUserManagerWrapper> _userManagerWrapper;
    private readonly Mock<ISignInManagerWrapper> _signInManagerWrapper;
    private readonly Mock<ITokenService> _tokenService;
    private readonly Mock<IMapper> _mapper;

    private readonly AccountController _accountController;


    public AccountControllerTests()
    {
        #region Dependencies
        _userManagerWrapper = new Mock<IUserManagerWrapper>();
        _signInManagerWrapper = new Mock<ISignInManagerWrapper>();
        _tokenService = new Mock<ITokenService>();
        _mapper = new Mock<IMapper>();
        #endregion

        #region System Under Test
        _accountController = new AccountController(
            _userManagerWrapper.Object,
            _signInManagerWrapper.Object,
            _tokenService.Object,
            _mapper.Object
            );
        #endregion
    }

    [Fact]
    public async Task GetCurrentUser_ReturnsUser()
    {
        // Arrange
        var user = new AppUser
        {
            DisplayName = "Test User",
            Address = new Address
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Street = "TestStreet",
                City = "TestCity",
                State = "TestState",
                ZipCode = "00000"
            }
        };

        var userDto = new UserDto
        {
            Email = user.Email,
            DisplayName = user.DisplayName,
            Token = "testToken"
        };

        _userManagerWrapper.Setup(um => um.GetUserByClaimsPrinciple(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        _tokenService.Setup(ts => ts.CreateToken(user))
            .Returns("testToken");

        // Act
        var result = await _accountController.GetCurrentUser();

        // Assert
        var actionResult = result.Should().BeOfType<ActionResult<UserDto>>().Subject;
        var userResult = actionResult.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task CheckEmailExistsAsync_ReturnsTrue()
    {
        // Arrange
        var emailToCheck = "existing@example.com";
        _userManagerWrapper.Setup(um => um.FindByEmailAsync(emailToCheck))
            .ReturnsAsync(new AppUser()); // Simulate an existing user

        // Act
        var result = await _accountController.CheckEmailExistsAsync(emailToCheck);

        // Assert
        result.Value.Should().Be(true);
    }

    [Fact]
    public async Task CheckEmailExistsAsync_ReturnsFalse()
    {
        // Arrange
        var emailToCheck = "existing@example.com";
        _userManagerWrapper.Setup(um => um.FindByEmailAsync(emailToCheck))
            .ReturnsAsync((AppUser)null); // Simulate an existing user

        // Act
        var result = await _accountController.CheckEmailExistsAsync(emailToCheck);

        // Assert
        result.Value.Should().Be(false);
    }

    [Fact]
    public async Task GetUserAddress_UserExists_ReturnsAddressDto()
    {
        // Arrange
        var user = new AppUser
        {
            Address = new Address
            {
                // Set address properties as needed for the test
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Street = "123 Main St",
                City = "Example City",
                State = "CA",
                ZipCode = "12345"
            }
        };

        _userManagerWrapper.Setup(um => um.GetUserByClaimsPrincipleWithAddress(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        // Act
        var result = await _accountController.GetUserAddress();

        // Assert
        result.Should().BeOfType<ActionResult<AddressDto>>();
    }

    [Fact]
    public async Task UpdateUserAddress_ReturnsAddressDto()
    {
        // Arrange
        var user = new AppUser
        {
            Address = new Address
            {
                // Set address properties as needed for the test
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Street = "123 Main St",
                City = "Example City",
                State = "CA",
                ZipCode = "12345"
            }
        };

        var addressDto = new AddressDto
        {
            FirstName = "Bob",
            LastName = "Doe",
            Street = "123 Main St",
            City = "Example City",
            State = "CA",
            Zipcode = "12345"
        };


        _userManagerWrapper.Setup(um => um
            .GetUserByClaimsPrincipleWithAddress(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);
        _userManagerWrapper.Setup(x => x.UpdateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Success);

        _mapper.Setup(m => m.Map<Address, AddressDto>(user.Address))
            .Returns(addressDto);

        // Act
        var result = await _accountController.UpdateUserAddress(addressDto);

        // Assert
        var okResult = result.Should().BeOfType<ActionResult<AddressDto>>();
    }

    [Fact]
    public async Task Login_ValidUserCredentials_ReturnsUserDto()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@gmail.com", Password = "12345" };
        var token = "testToken";

        var user = new AppUser
        {
            DisplayName = "Test User",
            Email = "test@gmail.com",
            Address = new Address
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                Street = "TestStreet",
                City = "TestCity",
                State = "TestState",
                ZipCode = "00000"
            }
        };

        _userManagerWrapper.Setup(um => 
            um.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);

        _signInManagerWrapper.Setup(sm => 
            sm.CheckPasswordSignInAsync(user, loginDto.Password, false))
            .ReturnsAsync(SignInResult.Success);
        _tokenService.Setup(ts => ts.CreateToken(user))
            .Returns(token);

        // Act
        var result = await _accountController.Login(loginDto);

        // Assert
        result.Should().BeOfType<ActionResult<UserDto>>();

        var userDto = result.Value;
        userDto.Should().NotBeNull();
        userDto.DisplayName.Should().Be(user.DisplayName);
        userDto.Email.Should().Be(user.Email);
        userDto.Token.Should().Be(token);
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "test@gmail.com", Password = "12345" };
        var token = "testToken";

        _userManagerWrapper.Setup(um =>
            um.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync((AppUser)null);

        // Act
        var result = await _accountController.Login(loginDto);

        // Assert
        result.Value.Should().BeOfType<UnauthorizedObjectResult>()
            .Which.Value.Should().BeOfType<ApiResponse>()
            .Which.StatusCode.Should().Be(401);
    }
}
