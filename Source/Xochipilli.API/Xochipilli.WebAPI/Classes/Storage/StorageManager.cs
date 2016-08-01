using Xochipilli.WebAPI.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Xochipilli.WebAPI.Classes.Storage
{
    public class StorageManager
    {
        public static string GetConnectionString()
        {
            string result = string.Empty;
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
            {
                result = Environment.GetEnvironmentVariable("conn-string");
            }
            else
            {
                result = ConfigurationManager.AppSettings["conn-string"];
            }
            return result;
        }

        public bool RegisterParticipant(CloudStorageAccount storageAccount, Participant participant)
        {
            if (participant == null)
                return false;

            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("Participant");
            table.CreateIfNotExists();

            Participant obj_participant = new Participant();
            obj_participant.PartitionKey = "Xochipilli_Participant";
            obj_participant.RowKey = Guid.NewGuid().ToString();
            obj_participant.FullName = participant.FullName;
            obj_participant.Gender = participant.Gender;
            obj_participant.Age = participant.Age;
            obj_participant.Weight = participant.Weight;
            obj_participant.TimeElapsed = participant.TimeElapsed;
            obj_participant.Intensity = participant.Intensity;
            obj_participant.FileName = participant.FileName;

            TableOperation operation = TableOperation.InsertOrReplace(obj_participant);
            table.Execute(operation);

            return true;
        }


    }
}