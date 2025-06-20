using MediatR;

using QrCodeGenerator.Ports;

namespace QrCodeGenerator.Features;

public static class GenerateQRCode
{
    public sealed record Command(string Url) : IRequest<string>;

    public sealed class Handler : IRequestHandler<Command, string>
    {
        private readonly IStorage _storage;
        private readonly IQRCodeGenerator _qrCodeGenerator;

        public Handler(IStorage storage, IQRCodeGenerator qrCodeGenerator)
        {
            _storage = storage;
            _qrCodeGenerator = qrCodeGenerator;
        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            var qrCodeBytes = _qrCodeGenerator.Generate(request.Url);

            var fileName = $"{Guid.NewGuid()}-{request.Url}.png";
            var contentType = "image/png";

            var fileUrl = await _storage.Upload(fileName, contentType, qrCodeBytes, cancellationToken);

            return fileUrl;
        }
    }

}