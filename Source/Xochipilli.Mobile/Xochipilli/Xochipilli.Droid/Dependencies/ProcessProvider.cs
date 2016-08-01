using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xochipilli.Classes;
using Xochipilli.Droid.Dependencies;
using Xochipilli.Entities;
using Xochipilli.Interfaces;

[assembly: Dependency(typeof(ProcessProvider))]

namespace Xochipilli.Droid.Dependencies
{
    public class ProcessProvider : IProcessProvider
    {
        public bool SaveLocalData(Participant participant)
        {
            var filename = "localdata";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, filename);

            string json = JsonConvert.SerializeObject(participant);

            using (var streamWriter = new StreamWriter(path, false))
            {
                streamWriter.WriteLine(json);
            }

            return true;
        }

        public Participant ReadLocalData()
        {
            var filename = "localdata";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, filename);
            string content = string.Empty;

            using (var streamReader = new StreamReader(path))
            {
                content = streamReader.ReadToEnd();
            }

            Participant result = JsonConvert.DeserializeObject<Participant>(content);

            return result;
        }

        public Stream GetDummyStream()
        {
            return MainActivity.GetStream();
        }

        public async Task<string> ProcessParticipantAsync(Stream stream)
        {
            Byte[] content = ToByteArrayFromStream(stream);
            return await UploadToBlobStorageAsync(content);
        }

        private byte[] ToByteArrayFromStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async Task<string> UploadToBlobStorageAsync(Byte[] blobContent)
        {
            string containerName = AzureStorageConstants.ContainerName;
            return await PutBlobAsync(containerName, blobContent);
        }

        private async Task<string> PutBlobAsync(string containerName, Byte[] blobContent)
        {
            string result = string.Empty;
            string identifier = string.Format("{0}.jpg", Guid.NewGuid().ToString());
            Int32 blobLength = blobContent.Length;
            const String blobType = "BlockBlob";

            String urlPath = String.Format("{0}/{1}", containerName, identifier);
            string queryString = "st=2016-07-28T22%3A22%3A00Z&se=2017-07-29T22%3A22%3A00Z&sp=rwdl&sv=2015-04-05&sr=c&sig=OZYN%2BbXMYBJvLAmc%2B%2F5JAe5X4Un3biFAiyPqkrYrFDE%3D";
            Uri uri = new Uri(AzureStorageConstants.BlobEndPoint + urlPath + '?' + queryString);

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-ms-blob-type", blobType);

            HttpContent requestContent = new ByteArrayContent(blobContent);
            HttpResponseMessage response = await client.PutAsync(uri, requestContent);

            if (response.IsSuccessStatusCode == true)
            {              
                result = identifier;
            }
            else
            {
                result = string.Empty;
            }

            return result;
        }
    }
}