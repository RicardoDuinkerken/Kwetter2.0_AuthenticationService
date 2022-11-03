using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Api.Dto;

public class DeleteAccountRequestDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}