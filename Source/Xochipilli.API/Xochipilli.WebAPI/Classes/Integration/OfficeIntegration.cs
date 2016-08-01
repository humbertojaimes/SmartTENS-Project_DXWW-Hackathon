using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Xochipilli.WebAPI.Entities;

namespace Xochipilli.WebAPI.Classes.Integration
{
    public class OfficeIntegration
    {
        public static bool Execute(Participant participant)
        {
            bool result = false;

            dynamic activityInfo = new
            {
                activityTitle = string.Empty,
                activitySubtitle = participant.FullName,
                activityText = string.Empty,
                activityImage = string.Empty
            };

            dynamic details = new
            {
                title = "Details",
                facts = new List<dynamic>()
                {
                    new { name="Intensity", value=participant.Intensity },

                    new { name="Time elapsed", value=participant.TimeElapsed }
                }
            };

            var sm = new
            {
                summary = "Participant",
                title = "New participant",
                sections = new List<dynamic> {
                    activityInfo,
                    details,
                },
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(sm);

            string urlwebhook = "https://outlook.office365.com/webhook/e5747919-bea9-407e-81df-31461dfce597@72f988bf-86f1-41af-91ab-2d7cd011db47/IncomingWebhook/852c8b43dbf64439aa73a4ccf5473186/9ec9015f-4b0d-47fd-9a10-0d14fe4a1ecf";
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.UploadStringAsync(new Uri(urlwebhook), "POST", json);
            }

            return result;
        }
    }
}