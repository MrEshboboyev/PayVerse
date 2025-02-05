namespace PayVerse.Application.Payments.Queries.Common.Responses;

public sealed record PaymentListResponse(IReadOnlyList<PaymentResponse> Payments);