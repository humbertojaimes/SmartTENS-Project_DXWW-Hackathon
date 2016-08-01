using System;
using Xamarin.Forms;
using Xochipilli.Droid.Dependencies;
using Xochipilli.Interfaces;

[assembly: Dependency(typeof(BluetoothService))]
namespace Xochipilli.Droid.Dependencies
{
    public class BluetoothService : IBluetoothService
    {
        public void Connect()
        {
            MainActivity.Connect();
        }

        public void WriteData(string data)
        {
            MainActivity.WriteData(data);
        }
    }
}