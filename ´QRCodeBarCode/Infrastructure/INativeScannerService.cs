namespace _QRCodeBarCode.Infrastructure
{
    public interface INativeScannerService
    {
        void ScannedValue(string code);
    }
}