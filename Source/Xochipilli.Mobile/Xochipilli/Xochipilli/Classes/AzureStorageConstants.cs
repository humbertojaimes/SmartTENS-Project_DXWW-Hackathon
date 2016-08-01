using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xochipilli.Classes
{
    public static class AzureStorageConstants
    {
        public static string Account = "xochipilli";
        public static string SharedKeyAuthorizationScheme = "SharedKey";
        public static string BlobEndPoint = "https://xochipilli.blob.core.windows.net/";
        public static string Key = "tWs/Sc5GsW1N22k7EaTSf8Lh1FEyXSIAXFmce80EBJa1e0YHvCHyTTz4Y43b6wPHf/SiIJgQg0C2WTYL0xLsxQ==";
        public static string ContainerName = "files";
        public static string FileLocation = BlobEndPoint + ContainerName;
    }
}
