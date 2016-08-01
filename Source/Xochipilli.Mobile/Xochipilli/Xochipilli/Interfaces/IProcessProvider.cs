using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xochipilli.Entities;

namespace Xochipilli.Interfaces
{
    public interface IProcessProvider
    {
        bool SaveLocalData(Participant participant);

        Participant ReadLocalData();

        Stream GetDummyStream();

        Task<string> ProcessParticipantAsync(Stream stream);
    }
}
