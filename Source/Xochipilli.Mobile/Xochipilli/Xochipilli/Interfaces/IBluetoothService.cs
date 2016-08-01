using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xochipilli.Interfaces
{
    public interface IBluetoothService
    {
        void Connect();

        void WriteData(string data);
    }
}
