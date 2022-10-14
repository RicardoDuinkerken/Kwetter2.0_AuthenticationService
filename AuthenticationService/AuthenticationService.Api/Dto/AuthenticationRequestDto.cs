using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Api.Dto;

public class AuthenticationRequestDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}