namespace AuthenticationService.Core.Exceptions;

public class FailedChangeEmailAttemptExpcetion : Exception
{
    public FailedChangeEmailAttemptExpcetion()
    {
        
    }

    public FailedChangeEmailAttemptExpcetion(string message) : base(message)
    {
        
    }
}