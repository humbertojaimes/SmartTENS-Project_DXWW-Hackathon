using System;
using System.Threading.Tasks;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Bluetooth;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Util;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;

namespace Xochipilli.Droid
{
    [Activity(Label = "Xochipilli", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static Stream input;
        private static BluetoothAdapter mBluetoothAdapter = null;
        private static BluetoothSocket btSocket = null;
        private static BluetoothDevice device;
        private static Stream outStream = null;
        private static string address = "98:D3:32:20:54:78";
        private static UUID MY_UUID = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        private static Stream inStream = null;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            input = Assets.Open(@"unknown.jpg");

            #region Resolver Init
            SimpleContainer container = new SimpleContainer();
            container.Register<IDevice>(t => AndroidDevice.CurrentDevice);
            container.Register<IDisplay>(t => t.Resolve<IDevice>().Display);
            container.Register<INetwork>(t => t.Resolve<IDevice>().Network);

            Resolver.SetResolver(container.GetResolver());
            #endregion

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            CheckBluetoothDevice();
        }

        public static Stream GetStream()
        {
            return input;
        }

        private void CheckBluetoothDevice()
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;

            if (!mBluetoothAdapter.Enable())
            {
                Toast.MakeText(this, "Bluetooth disabled",
                ToastLength.Short).Show();
            }

            if (mBluetoothAdapter == null)
            {
                Toast.MakeText(this, "Bluetooth device is occupied or non-existant", ToastLength.Short).Show();
            }
        }

        public static void Connect()
        {
            mBluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            device = mBluetoothAdapter.GetRemoteDevice(address);
            System.Diagnostics.Debug.WriteLine("Connecting to" + device.Name);
            mBluetoothAdapter.CancelDiscovery();
            try
            {
                btSocket = device.CreateRfcommSocketToServiceRecord(MY_UUID);
                btSocket.Connect();
                System.Diagnostics.Debug.WriteLine("Connection established");
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                try
                {
                    btSocket.Close();
                }
                catch (System.Exception)
                {
                    System.Diagnostics.Debug.WriteLine("Imposible Conectar");
                }
                System.Diagnostics.Debug.WriteLine("Socket Creado");
            }
            BeginListenForData();
        }

        public static void BeginListenForData()
        {
            try
            {
                inStream = btSocket.InputStream;
            }
            catch (System.IO.IOException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            Task.Factory.StartNew(() => {
                byte[] buffer = new byte[1024];
                int bytes;
                while (true)
                {
                    try
                    {
                        bytes = inStream.Read(buffer, 0, buffer.Length);
                        if (bytes > 0)
                        {
                            //RunOnUiThread(() => {
                            //    string valor = System.Text.Encoding.ASCII.GetString(buffer);
                            //    Result.Text = Result.Text + "\n" + valor;
                            //});
                        }
                    }
                    catch (Java.IO.IOException)
                    {
                        //RunOnUiThread(() => {
                        //    Result.Text = string.Empty;
                        //});
                        //break;
                    }
                }
            });
        }

        public static void WriteData(string data)
        {
            try
            {
                outStream = btSocket.OutputStream;
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error al enviar" + e.Message);
            }

            Java.Lang.String message = new Java.Lang.String(data);

            byte[] msgBuffer = message.GetBytes();

            try
            {
                outStream.Write(msgBuffer, 0, msgBuffer.Length);
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error al enviar" + e.Message);
            }
        }
    }
}

