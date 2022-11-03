using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Api.Dto;

public class ChangeEmailRequestDto
{
    [Required]
    [EmailAddress]
    public string OldEmail { get; set; }
    
    [Required]
    [EmailAddress]
    public string NewEmail { get; set; }
    
    [Required]
    [EmailAddress]
    public string ConfirmNewEmail { get; set; }
}