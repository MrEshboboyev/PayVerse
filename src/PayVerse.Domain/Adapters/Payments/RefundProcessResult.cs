﻿namespace PayVerse.Domain.Adapters.Payments;

public class RefundProcessResult
{
    public bool IsSuccess { get; set; }
    public string RefundTransactionId { get; set; }
    public string ErrorMessage { get; set; }
    public DateTime RefundedDate { get; set; }
}
