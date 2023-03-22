using _QRCodeBarCode;
using _QRCodeBarCode.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeScannerService))]
namespace _QRCodeBarCode
{
    public class NativeScannerService : INativeScannerService
    {
        public void ScannedValue(string code)
        {
            MessagingCenter.Instance.Send<INativeScannerService, string>(this, "Scanned", code);
        }
    }
}