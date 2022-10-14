using AuthenticationService.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Dal;

public class AuthenticationServiceContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public AuthenticationServiceContext()
    {
            
    }

    public AuthenticationServiceContext(DbContextOptions<AuthenticationServiceContext> options) : base(options)
    {
            
    }

}