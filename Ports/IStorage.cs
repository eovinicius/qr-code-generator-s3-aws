namespace QrCodeGenerator.Ports;

public interface IStorage
{
    Task<string> Upload(string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default);
}
