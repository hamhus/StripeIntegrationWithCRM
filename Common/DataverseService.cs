using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Hackathon.StripePayment.Common
{
    public class DataverseService
    {
        public ServiceClient GetDataVerseService(ILogger log)
        {
            ServiceClient service = null;
            //Enter your CRM URL, ClientId, clientsecret for Development
            //For production - use managed identity
            string url = "";
            string clientId = "";
            string clientSecret = "";

            try
            {
                if (!string.IsNullOrEmpty(clientSecret))
                {
                    string connection = "AuthType=ClientSecret;url=" + url + ";ClientId=" + clientId + ";ClientSecret=" + clientSecret;

                    service = new ServiceClient(connection);
                    string token = service.CurrentAccessToken;

                    //service = new ServiceClient()
                    if (service.IsReady)
                    {
                        return service;
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
            }
            return service;
        }
    }
}
