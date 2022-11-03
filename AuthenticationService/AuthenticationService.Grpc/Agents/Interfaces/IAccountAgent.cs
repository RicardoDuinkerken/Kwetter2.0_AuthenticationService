namespace AuthenticationService.Grpc.Agents.Interfaces;

public interface IAccountAgent
{
    Task<AccountResponse> CreateAccount(CreateAccountRequest account);
}