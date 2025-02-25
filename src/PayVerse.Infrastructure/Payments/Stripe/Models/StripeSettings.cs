﻿namespace PayVerse.Infrastructure.Payments.Stripe.Models;

public class StripeSettings
{
    public string PublishableKey { get; set; }
    public string SecretKey { get; set; }
    public string WebhookSecret { get; set; }
}
