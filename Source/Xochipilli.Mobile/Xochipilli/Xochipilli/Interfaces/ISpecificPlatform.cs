using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xochipilli.Interfaces
{
    public interface ISpecificPlatform
    {
        bool CheckIfSimulator();

        void CloseApplication();
    }
}
