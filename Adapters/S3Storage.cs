using Amazon.S3;
using Amazon.S3.Model;

using QrCodeGenerator.Ports;

namespace QrCodeGenerator.Adapters;

public class S3Storage : IStorage
{
    private readonly IAmazonS3 _client;
    private const string BUCKET_NAME = "bucket-qrcodes";


    public S3Storage(IAmazonS3 client)
    {
        _client = client;
    }

    public async Task<string> Upload(string fileName, string contentType, byte[] content, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream(content);

        var request = new PutObjectRequest
        {
            Key = fileName,
            InputStream = stream,
            ContentType = "image/png",
            BucketName = BUCKET_NAME,
        };

        await _client.PutObjectAsync(request, cancellationToken);

        string fileUrl = $"https://{BUCKET_NAME}.s3.amazonaws.com/{fileName}";
        return fileUrl;
    }
}