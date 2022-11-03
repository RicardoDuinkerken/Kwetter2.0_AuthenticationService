namespace AuthenticationService.Core.Exceptions;

public class FailedChangePasswordAttemptException : Exception
{
    public FailedChangePasswordAttemptException()
    {
        
    }

    public FailedChangePasswordAttemptException(string message) : base(message)
    {
        
    }
}