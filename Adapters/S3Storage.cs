using Amazon.S3;
using Amazon.S3.Transfer;
using QrCodeGenerator.Ports;

namespace QrCodeGenerator.Adapters;

public class S3Storage : IStorage
{
    private readonly IAmazonS3 _client;
    private const string BUCKET_NAME = "321321";


    public S3Storage(IAmazonS3 client)
    {
        _client = client;
    }

    public async Task<string> Upload(string fileName, string contentType, byte[] content, CancellationToken cancellationToken)
    {
        var fileTransferUtility = new TransferUtility(_client);

        using var stream = new MemoryStream(content);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = stream,
            Key = fileName,
            BucketName = BUCKET_NAME,
            ContentType = "image/png"
        };

        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

        string fileUrl = $"https://{BUCKET_NAME}.s3.amazonaws.com/{fileName}";
        return fileUrl;
    }
}


