namespace AuthenticationService.Core.Exceptions;

public class FailedDeleteAttemptException : Exception
{
    public FailedDeleteAttemptException()
    {
        
    }

    public FailedDeleteAttemptException(string message) : base(message)
    {
        
    }
}