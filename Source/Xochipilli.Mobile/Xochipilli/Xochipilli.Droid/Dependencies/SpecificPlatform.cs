using Android.OS;
using Xamarin.Forms;
using Xochipilli.Droid.Dependencies;
using Xochipilli.Interfaces;

[assembly: Dependency(typeof(SpecificPlatform))]

namespace Xochipilli.Droid.Dependencies
{
    public class SpecificPlatform : ISpecificPlatform
    {
        public bool CheckIfSimulator()
        {
            if (Build.Fingerprint != null)
            {
                if (Build.Fingerprint.Contains("vbox") ||
                    Build.Fingerprint.Contains("generic"))
                    return true;
            }
            return false;
        }

        public void CloseApplication()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}