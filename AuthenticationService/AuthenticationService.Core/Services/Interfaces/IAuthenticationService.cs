namespace AuthenticationService.Core.Services.interfaces;

public interface IAuthenticationService
{
    Task<string> Login(string email, string password);
    Task<bool> Register(string email, string password);
    Task<bool> DeleteAccount(string email);
    Task<bool> ChangeEmail(string oldEmail, string newEmail);
    Task<bool> ChangePassword(string email, string password);
}