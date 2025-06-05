using QrCodeGenerator.Ports;

namespace QrCodeGenerator.Adapters;

public class QrCodeGenerator : IQRCodeGenerator
{
    public byte[] Generate(string text)
    {
        using var qrGenerator = new QRCoder.QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(text, QRCoder.QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCoder.PngByteQRCode(qrData);
        return qrCode.GetGraphic(20);
    }
}
