using Xochipilli.WebAPI.Classes;
using Xochipilli.WebAPI.Classes.Storage;
using Xochipilli.WebAPI.Filters;
using Xochipilli.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using Xochipilli.WebAPI.Classes.Integration;

namespace Xochipilli.WebAPI.Controllers
{
    /// <summary>
    /// controller responsible to provide data to the mobile apps.
    /// </summary>
    /// 

    public class ProviderController : ApiController
    {

        /// <summary>
        /// register record.
        /// </summary>
        /// <returns></returns>
        [CustomException]
        [HttpPost]
        [Route("api/saveparticipant")]
        public IHttpActionResult SaveParticipant(Participant participant)
        {
            var storageConnectionString = StorageManager.GetConnectionString();
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(storageConnectionString);

            StorageManager sm = new StorageManager();
            var myTask = Task<bool>.Factory.StartNew(() => sm.RegisterParticipant(storageAccount, participant));
            bool result = myTask.Result;

            //Office integration.
            var myTaskOffice = Task<bool>.Factory.StartNew(() => OfficeIntegration.Execute(participant));

            return Json(result);
        }
    }
}