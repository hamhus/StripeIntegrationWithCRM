using AB.Hackathon.StripePayment.Common;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AB.Hackathon.StripePayment.BusinessLogic
{
    public class ProcessPayment
    {

        public void ProcessPaymentInCrm(ILogger log, string requestBody)
        {
            DataverseService orgService = new DataverseService();
            ServiceClient service = orgService.GetDataVerseService(log);
            if (service != null && service.IsReady)
            {
                log.LogInformation("Connected to CRM");
                dynamic data = DeserializeJsonData(requestBody);

                if (data != null && data?.data != null)
                {
                    dynamic paymentJsonData = DeserializeJsonData(Convert.ToString(data?.data));                    //dynamic key = CommonLogic.DeserializeJsonData(Convert.ToString(data?.key));
                    if (paymentJsonData != null && paymentJsonData?.@object != null)
                    {
                        dynamic dataObject = DeserializeJsonData(Convert.ToString(paymentJsonData?.@object));
                        if (dataObject != null)
                        {
                            var charges_received = dataObject?.amount_received;

                            dynamic paydata = DeserializeJsonData(Convert.ToString(dataObject?.charges));

                            var paydata1 = paydata?.data;

                            dynamic paydataJson = DeserializeJsonData(Convert.ToString(paydata1[0]));

                            dynamic paydataJson1 = DeserializeJsonData(Convert.ToString(paydataJson?.billing_details));

                            var name = paydataJson1?.name;
                            var email = paydataJson1?.email;

                            var contactId = CreateContact(name.ToString(), email.ToString(), service, log);

                            if (contactId != new Guid())
                            {
                                Guid invoiceId  = CreateInvoice(Convert.ToDecimal(charges_received.ToString()), contactId, service, log);
                                CreateInvoiceLine(Convert.ToDecimal(charges_received.ToString()), invoiceId, service, log);
                            }
                        }
                    }
                }

            }
        }

        public static dynamic DeserializeJsonData(string postedJsonReq)
        {
            dynamic data = JsonConvert.DeserializeObject(postedJsonReq);
            
            return data;
        }

        public Guid CreateContact(string name, string email, ServiceClient service, ILogger log)
        {
            Entity contact = new Entity("contact");
            contact.Attributes["fullname"] = name;
            contact.Attributes["lastname"] = name;
            contact.Attributes["emailaddress1"] = email;

            //log.LogInformation("Contact created : " + Convert.ToString(invoiceId));

            return service.Create(contact);

        }
        public Guid CreateInvoice(decimal amount, Guid contactId, ServiceClient service, ILogger log)
        {

            Entity invoice = new Entity("invoice");
            invoice.Attributes["totallineitemamount"] = new Money(amount);
            invoice.Attributes["totalamount"] = new Money(amount);
            invoice.Attributes["customerid"] = new EntityReference("contact", contactId);
            invoice.Attributes["name"] = "Stripe";
            invoice.Attributes["pricelevelid"] = new EntityReference("pricelevel", new Guid("6357017d-d5b1-ea11-a812-000d3a5a79a0"));
            
            var invoiceId = service.Create(invoice);

            log.LogInformation("Invoice created : " + Convert.ToString(invoiceId));

            return invoiceId;
        }

        public void CreateInvoiceLine(decimal amount, Guid invoiceId, ServiceClient service, ILogger log)
        {

            Entity invoice = new Entity("invoicedetail");
            invoice.Attributes["isproductoverridden"] = true;
            invoice.Attributes["productdescription"] = "Stripe";
            invoice.Attributes["priceperunit"] = new Money(amount);
            invoice.Attributes["quantity"] = 1;
            invoice.Attributes["invoiceid"] = new EntityReference("invoice", invoiceId);
            var invoiceLineId = service.Create(invoice);

            log.LogInformation("Invoice Detail created : " + Convert.ToString(invoiceLineId));
        }
    }
}
