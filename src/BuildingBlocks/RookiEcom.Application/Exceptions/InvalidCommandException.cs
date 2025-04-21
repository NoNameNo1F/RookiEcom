namespace RookiEcom.Application.Exceptions;

public class InvalidCommandException : Exception
{
    public List<string> Errors { get; }

    public InvalidCommandException(List<string> errors)
    {
        Errors = errors;
    }
}