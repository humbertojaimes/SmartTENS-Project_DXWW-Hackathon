using ObjCRuntime;
using System;
using Xamarin.Forms;
using Xochipilli.Interfaces;
using Xochipilli.iOS.Dependencies;

[assembly: Dependency(typeof(SpecificPlatform))]
namespace Xochipilli.iOS.Dependencies
{
    public class SpecificPlatform : ISpecificPlatform
    {
        public bool CheckIfSimulator()
        {
            if (Runtime.Arch == Arch.SIMULATOR)
                return true;
            return false;
        }

        public void CloseApplication()
        {
            throw new NotImplementedException();
        }
    }
}