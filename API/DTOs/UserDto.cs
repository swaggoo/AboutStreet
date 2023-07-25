using System.Reflection.Metadata.Ecma335;

namespace API.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string Token { get; set; }
}