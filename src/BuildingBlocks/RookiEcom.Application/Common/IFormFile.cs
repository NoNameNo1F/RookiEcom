namespace RookiEcom.Application.Common;

public class IFormFile
{
    public Stream Content { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }

    public string GetPathWithFileName(string basePath)
    {
        var uniqueFileName = Path.GetRandomFileName();
        var uniqueFileWithoutExtension = Path.GetFileNameWithoutExtension(uniqueFileName);
        var fileExtension = Path.GetExtension(FileName);

        return $"{basePath}/{uniqueFileWithoutExtension}{fileExtension}";
    }
}
