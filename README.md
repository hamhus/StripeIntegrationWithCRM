# StripeIntegrationWithCRM
1. Register an account on Stripe
2. Create a payment page with a credit card
3. Stripe gives you access to Webhook to perform integration. The webhooks lets you choose triggers to fire your webhook
4. Create an Azure function that accepts this json when a webhook is fired.
5. Parson the JSON file in your Azure function and create/update invoices in CRM
