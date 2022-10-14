namespace AuthenticationService.Dal.Models;

public class Account
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}