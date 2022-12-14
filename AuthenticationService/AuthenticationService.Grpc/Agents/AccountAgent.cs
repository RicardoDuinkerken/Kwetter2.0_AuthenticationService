using AuthenticationService.Grpc.Agents.Interfaces;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Grpc.Agents;

public class AccountAgent : IAccountAgent
{
    private readonly string _address;
    private readonly ILogger<AccountAgent> _logger;
    
    public AccountAgent(IConfiguration configuration, ILogger<AccountAgent> logger)
    {
        _address = configuration["AccountService"] ?? "http://localhost:5003";
        Console.WriteLine(_address);
        _logger = logger;
    }
    
    public async Task<AccountResponse> CreateAccount(CreateAccountRequest account)
    {
        using var channel = GrpcChannel.ForAddress(_address);
        var client = new AccountService.AccountServiceClient(channel);
        return await client.CreateAccountAsync(account);
    }
}