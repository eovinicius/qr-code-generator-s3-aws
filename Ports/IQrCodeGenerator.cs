namespace QrCodeGenerator.Ports;

public interface IQRCodeGenerator
{
    byte[] Generate(string text);
}
