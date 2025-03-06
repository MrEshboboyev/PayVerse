using PayVerse.Domain.Entities.Payments;
using PayVerse.Domain.Enums.Payments;
using PayVerse.Domain.Errors;
using PayVerse.Domain.Mementos;
using PayVerse.Domain.Mementos.Payments;
using PayVerse.Domain.Repositories;
using PayVerse.Domain.Repositories.Payments;
using PayVerse.Domain.Shared;

namespace PayVerse.Application.Mementos.Payments;

public class PaymentHistoryService(
    MementoCaretaker<PaymentMemento> caretaker,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork) : IPaymentHistoryService
{
    public async Task<Result> SavePaymentStateAsync(Payment payment, string metadata = "")
    {
        try
        {
            var memento = payment.CreateMemento(metadata);
            caretaker.SaveMemento(payment.Id, memento);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(DomainErrors.Payment.SaveStateError(ex.Message));
        }
    }

    public async Task<Result<Payment>> RestorePaymentStateAsync(Guid paymentId, int version)
    {
        try
        {
            if (!caretaker.HasMementos(paymentId))
            {
                return Result.Failure<Payment>(
                    DomainErrors.Payment.NoHistoryFound(paymentId));
            }

            var payment = await paymentRepository.GetByIdAsync(paymentId);
            if (payment is null)
            {
                return Result.Failure<Payment>(
                    DomainErrors.Payment.NotFound(paymentId));
            }

            var memento = caretaker.GetMemento(paymentId, version);
            var result = payment.RestoreFromMemento(memento);

            if (result.IsSuccess)
            {
                await unitOfWork.SaveChangesAsync();
                return Result.Success(payment);
            }

            return Result.Failure<Payment>(result.Error);
        }
        catch (Exception ex)
        {
            return Result.Failure<Payment>(
                DomainErrors.Payment.RestoreStateError(ex.Message));
        }
    }

    public async Task<Result<Payment>> RestoreLatestPaymentStateAsync(Guid paymentId)
    {
        try
        {
            if (!caretaker.HasMementos(paymentId))
            {
                return Result.Failure<Payment>(DomainErrors.Payment.NoHistoryFound(paymentId));
            }

            var payment = await paymentRepository.GetByIdAsync(paymentId);
            if (payment is null)
            {
                return Result.Failure<Payment>(DomainErrors.Payment.NotFound(paymentId));
            }

            var memento = caretaker.GetLatestMemento(paymentId);
            var result = payment.RestoreFromMemento(memento);

            if (result.IsSuccess)
            {
                await unitOfWork.SaveChangesAsync();
                return Result.Success(payment);
            }

            return Result.Failure<Payment>(result.Error);
        }
        catch (Exception ex)
        {
            return Result.Failure<Payment>(DomainErrors.Payment.RestoreStateError(ex.Message));
        }
    }

    public async Task<Result<List<PaymentHistoryDto>>> GetPaymentHistoryAsync(Guid paymentId)
    {
        try
        {
            if (!caretaker.HasMementos(paymentId))
            {
                return Result.Success(new List<PaymentHistoryDto>());
            }

            var mementos = caretaker.GetAllMementos(paymentId);
            var historyItems = new List<PaymentHistoryDto>();

            for (int i = 0; i < mementos.Count; i++)
            {
                var memento = mementos[i];
                var state = System.Text.Json.JsonSerializer.Deserialize<PaymentState>(memento.GetState());

                if (state == null)
                {
                    return Result.Failure<List<PaymentHistoryDto>>(DomainErrors.Payment.DeserializationFailed);
                }

                historyItems.Add(new PaymentHistoryDto(
                    i,
                    memento.CreatedAt,
                    memento.GetMetadata(),
                    state.Status,
                    state.Amount,
                    state.Currency));
            }

            return Result.Success(historyItems);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<PaymentHistoryDto>>(DomainErrors.Payment.GetHistoryError(ex.Message));
        }
    }

    // Helper class for deserializing payment state
    private class PaymentState
    {
        public PaymentStatus Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
