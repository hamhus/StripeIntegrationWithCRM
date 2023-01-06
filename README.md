# Stripe Integration With CRM
1. Register an account on Stripe
2. Create a payment page with a credit card
3. Stripe gives you access to Webhook to perform integration. The webhooks lets you choose triggers to fire your webhook when an action (payment) is performed.
4. Create an Azure function that accepts this json when a webhook is fired.
5. Parson the JSON file in your Azure function and create/update invoices in CRM

Rough Architecture:

![StripeIntegrationWithCRMArchitecture](https://user-images.githubusercontent.com/18374071/210919833-ed1df7c3-228c-40a0-ad1d-d80c3d647a37.JPG)

Azure function receives the JSON from Stripe:

![AzureFunction_StripePayment](https://user-images.githubusercontent.com/18374071/210919877-b1f9d845-a6fd-4408-ad23-d9bfcf2aba74.JPG)
