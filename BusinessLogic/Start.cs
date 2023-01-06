using AB.Hackathon.StripePayment.Common;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Hackathon.StripePayment.BusinessLogic
{
    public class Start
    {

        public void Payment(ILogger log)
        {
            DataverseService service = new DataverseService();
            ServiceClient orgService = service.GetDataVerseService(log);

            ProcessPayment payment = new ProcessPayment();
            //payment.ProcessPaymentInCrm(log, orgService);
        }
    }
}
