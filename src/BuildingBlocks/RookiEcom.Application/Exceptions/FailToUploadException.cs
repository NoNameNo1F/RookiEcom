namespace RookiEcom.Application.Exceptions;

public class FailToUploadException : Exception
{
    public FailToUploadException() : base($"Failed to upload image to Blob Storage.")
    {
    }
}