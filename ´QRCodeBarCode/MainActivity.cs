using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using static Android.Telephony.CarrierConfigManager;
using _QRCodeBarCode.Infrastructure;
using Xamarin.Forms;

namespace _QRCodeBarCode
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            MessagingCenter.Instance.Subscribe<INativeScannerService, string>(this, "Scanned", INativeScannerServiceMethod);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            Android.Views.View view = (Android.Views.View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async void INativeScannerServiceMethod(INativeScannerService sender, string e)
        {
            
        }
        public override bool OnKeyMultiple([GeneratedEnum] Android.Views.Keycode keyCode, int repeatCount, Android.Views.KeyEvent e)
        {
            DependencyService.Get<INativeScannerService>().ScannedValue(e.Characters);

            return base.OnKeyMultiple(keyCode, repeatCount, e);
        }

        string barcode = "";
        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            switch (keyCode)
            {
                case Keycode.Cut:
                    barcode = string.Empty;

                    break;
                case Keycode.Enter:
                    if (string.IsNullOrEmpty(barcode))
                        return false;

                    DependencyService.Get<INativeScannerService>().ScannedValue(barcode);
                    barcode = string.Empty;

                    return true;

                case Keycode.Back:
                    base.OnBackPressed();
                    return true;
                default:
                    var c = (char)e.GetUnicodeChar(MetaKeyStates.None);
                    if (c != '\0')
                        barcode += c.ToString().ToUpper();

                    break;
            }

            return false;
        }

    }
}
