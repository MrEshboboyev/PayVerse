using PayVerse.Domain.Entities.Notifications;
using PayVerse.Domain.Shared;

namespace PayVerse.Domain.Errors;

/// <summary> 
/// Defines and organizes domain-specific errors. 
/// </summary>
public static class DomainErrors
{
    #region Users

    #region Entities

    public static class User
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "User.NotFound",
            $"The user with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Users.NotExist",
            $"There is no users");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials are invalid");

        public static readonly Error InvalidPasswordChange = new(
            "User.InvalidPasswordChange",
            "The password change operation is invalid.");

        public static readonly Error InvalidRoleName = new(
            "User.InvalidRoleName",
            "The specified role is invalid.");

        public static readonly Func<int, Error> RoleNotAssigned = roleId => new Error(
            "User.RoleNotAssigned",
            $"The specified role with ID {roleId} is not assigned to the user.");

        public static readonly Func<int, Error> NoUsersFoundForRole = roleId => new Error(
            "User.NoUsersFoundForRole",
            $"No users found for the role with the identifier {roleId}.");

        public static readonly Func<string, Error> NotFoundForEmail = email => new Error(
            "User.NotFoundForEmail",
            $"The user with the email {email} was not found.");

        public static readonly Func<string, Error> NoUsersFoundForSearchTerm = searchTerm => new Error(
            "User.NoUsersFoundForSearchTerm",
            $"No users were found matching the search term '{searchTerm}'.");
    }

    public static class Role
    {
        public static readonly Func<int, Error> NotFound = id => new Error(
            "Role.NotFound",
            $"The role with the identifier {id} was not found.");

        public static readonly Error CannotBeNull = new Error(
            "Role.CannotBeNull",
            "The role cannot be null.");

        public static readonly Error NotAssignedToUser = new Error(
            "Role.NotAssignedToUser",
            "The role is not assigned to this user.");
    }

    #endregion

    #region Value Objects

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");

        public static readonly Func<string, Error> SendingFailed = errorMessage => new Error(
            "Email.SendingFailed",
            $"Failed to send email: {errorMessage}");

        public static readonly Func<string, Error> SendingFailedWithAttachment = errorMessage => new Error(
            "Email.SendingFailedWithAttachment",
            $"Failed to send email with attachment: {errorMessage}");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    #endregion

    #region Roles

    public static class Admin
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Admin.NotFound",
            $"The admin with the identifier {id} was not found.");
    }

    public static class BusinessUser
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "BusinessUser.NotFound",
            $"The business user with the identifier {id} was not found.");
    }

    public static class IndividualUser
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "IndividualUser.NotFound",
            $"The individual user with the identifier {id} was not found.");
    }

    #endregion

    #endregion
    
    #region Invoice
    
    #region Entities

    public static class Invoice
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Invoice.NotFound",
            $"The invoice with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Invoice.NotExist",
            $"There is no invoices");
        
        public static readonly Func<Guid, Error> ItemNotFound = id => new Error(
            "Invoice.ItemNotFound",
            $"The invoice item with the identifier {id} was not found in this invoice.");
    }

    public static class InvoiceItem
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "InvoiceItem.NotFound",
            $"The invoice item with the identifier {id} was not found.");
    }

    #endregion

    #region Value Objects

    public static class Amount
    {
        public static readonly Error Negative = new(
            "Amount.Negative",
            "Amount must not be negative.");
    }
    
    public static class InvoiceDate
    {
        public static readonly Error FutureDate = new(
            "InvoiceDate.FutureDate",
            "Invoice date must not be in the future.");
    }

    public static class InvoiceNumber
    {
        public static readonly Error Empty = new(
            "InvoiceNumber.Empty",
            "Invoice number must not be empty.");

        public static readonly Error TooLong = new(
            "InvoiceNumber.TooLong",
            "Invoice number must not be too long.");
    }

    #endregion
    
    #endregion
    
    #region VirtualAccount
    
    #region Entities

    public static class VirtualAccount
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "AccountNumber.NotFound",
            $"The account number with the identifier {id} was not found.");
        
        public static readonly Error FromOrToAccountNotActive = new(
            "VirtualAccount.FromOrToAccountNotActive",
            "Both accounts must be active for a transfer.");

        public static readonly Func<Guid, Error> AccountAlreadyClosed = id => new Error(
            "VirtualAccount.AccountAlreadyClosed",
            $"The account [ID : {id}] is already closed.");

        public static readonly Func<Guid, Error> AccountAlreadyClosedOrFrozen = id => new Error(
            "VirtualAccount.AccountAlreadyClosedOrFrozen",
            $"The account [ID : {id}] is either already closed or frozen.");

        public static readonly Func<Guid, Error> AccountNotFrozen = id => new Error(
            "VirtualAccount.AccountNotFrozen",
            $"The account [ID : {id}] is not frozen.");

        public static readonly Func<Guid, Error> InsufficientFunds = id => new Error(
            "VirtualAccount.InsufficientFunds",
            $"Insufficient funds in account with [ID : {id}].");

        public static readonly Func<decimal, Error> FinalBalanceFailure = amount => new Error(
            "VirtualAccount.FinalBalanceFailure",
            $"Unable to create balance with amount {amount}.");
        
        public static readonly Func<Guid, Error> ItemNotFound = id => new Error(
            "Invoice.ItemNotFound",
            $"The invoice item with the identifier {id} was not found in this invoice.");
    }

    #endregion

    #region Value Objects

    public static class AccountNumber
    {
        public static readonly Error Empty = new(
            "AccountNumber.Empty",
            "Account number must not be empty.");
        
        public static readonly Error InvalidLength = new(
            "AccountNumber.InvalidLength",
            "Account number must have a valid length.");
    }

    public static class Balance
    {
        public static readonly Error Negative = new(
            "Balance.Negative",
            "Balance must not be negative.");
    }

    public static class Currency
    {
        public static readonly Error Invalid = new(
            "Currency.Invalid",
            "Currency must be valid.");
    }
    
    #endregion
    
    #endregion
    
    #region Payments
    
    #region Entities

    public static class Payment
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Payment.NotFound",
            $"The payment with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Payment.NotExist",
            $"There is no payments");
    }   

    #endregion

    #region Value Objects

    public static class PaymentAmount
    {
        public static readonly Error Invalid = new(
            "PaymentAmount.Invalid",
            "Payment amount must be valid.");
    }

    #endregion

    #region Adapters

    public static class PayPal
    {
        public static readonly Func<string, Error> ProcessingFailed = message => new Error(
            "PayPal.ProcessingFailed",
            $"PayPal payment processing failed: {message}");
    }

    public static class Stripe
    {
        public static readonly Func<string, Error> ProcessingFailed = message => new Error(
            "Stripe.ProcessingFailed",
            $"Stripe payment processing failed: {message}");
    }

    #endregion

    #endregion

    #region Wallet

    #region Entities

    public static class Wallet
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Wallet.NotFound",
            $"The wallet with the identifier {id} was not found.");

        public static readonly Func<int, int, Error> InsufficientLoyaltyPoints = (
            requiredPoints,
            availablePoints) => new Error(
            "Wallet.InsufficientLoyaltyPoints",
            $"Insufficient loyalty points." +
            $" Required: {requiredPoints}," +
            $" Available: {availablePoints}.");
        
        public static Error TransactionNotFound(Guid transactionId) => new(
            "Wallet.TransactionNotFound",
            $"Transaction with ID {transactionId} was not found.");
    }
    
    #endregion

    #region Value Objects
    
    public static class WalletBalance
    {
        public static readonly Error Negative = new(
            "WalletBalance.Negative",
            "Wallet balance must not be negative.");
    }
    
    #endregion

    #endregion
    
    #region Security
    
    public static class SecurityIncident
    {
        public static readonly Func<Guid, Error> IncidentAlreadyResolved = incidentId => new Error(
            "SecurityIncident.AlreadyResolved",
            $"The security incident with ID {incidentId} is already resolved.");

        public static readonly Func<Guid, Error> IncidentAlreadyEscalated = incidentId => new Error(
            "SecurityIncident.AlreadyEscalated",
            $"The security incident with ID {incidentId} is already escalated.");
    }
    
    #endregion
    
    #region Notifications
    
    #region Entities
    
    public static class Notification
    {
        public static readonly Func<Guid, Error> NotFound = notificationId => new Error(
            "Notification.NotFound",
            $"The notification with ID {notificationId} was not found.");
        
        public static readonly Func<Guid, Error> AlreadyRead = notificationId => new Error(
            "Notification.AlreadyRead",
            $"The notification with ID {notificationId} has already been marked as read.");
        
        public static readonly Func<Guid, Error> AlreadySent = notificationId => new Error(
            "Notification.AlreadySent",
            $"The notification with ID {notificationId} has already been sent.");

        public static readonly Func<Guid, Error> NotSentYet = notificationId => new Error(
            "Notification.NotSentYet",
            $"The notification with ID {notificationId} has not been sent yet.");

        public static readonly Func<Guid, Error> CannotModifySentNotification = notificationId => new Error(
            "Notification.CannotModifySentNotification",
            $"The notification with ID {notificationId} cannot be modified because" +
            $" it has already been sent.");
    }
    
    #endregion
    
    #region Value objects
    
    public static class NotificationMessage
    {
        // Existing error definitions...

        public static readonly Func<string, Error> Empty = message => new Error(
            "NotificationMessage.Empty",
            $"Notification message cannot be empty. Provided message: '{message}'");

        public static readonly Func<string, int, Error> TooLong = (message, maxLength) => new Error(
            "NotificationMessage.TooLong",
            $"Notification message cannot exceed {maxLength} characters. Provided message length: {message.Length}.");
    }

    public static class NotificationType
    {
        // Existing error definitions...

        public static readonly Func<string, Error> Invalid = type => new Error(
            "NotificationType.Invalid",
            $"Notification type is invalid. Provided type: '{type}'");

        public static readonly Func<string, IEnumerable<string>, Error> Unsupported = (type, validTypes) => new Error(
            "NotificationType.Unsupported",
            $"Notification type '{type}' is not supported. Supported types are: {string.Join(", ", validTypes)}");
    }
    
    #endregion
    
    #endregion
    
    #region Reports
    
    #region Entities
    
    public static class FinancialReport
    {
        public static readonly Func<Guid, Error> NotFound = reportId => new Error(
            "FinancialReport.NotFound",
            $"The report with ID {reportId} was not found.");
        
        public static readonly Func<Guid, Error> CannotMarkAsCompleted = reportId => new Error(
            "FinancialReport.CannotMarkAsCompleted",
            $"The report with ID {reportId} cannot be marked as completed" +
            $" because it is not in the pending status.");

        public static readonly Func<Guid, Error> CannotMarkAsFailed = reportId => new Error(
            "FinancialReport.CannotMarkAsFailed",
            $"The report with ID {reportId} cannot be marked as failed" +
            $" because it is not in the pending status.");

        public static readonly Func<Guid, Error> ReportNotCompleted = reportId => new Error(
            "FinancialReport.ReportNotCompleted",
            $"The report with ID {reportId} is not completed.");
    }

    #endregion
    
    #region Value objects
    
    public static class ReportPeriod
    {
        public static readonly Func<DateOnly, DateOnly, Error> InvalidDateRange = (start, end) => new Error(
            "ReportPeriod.InvalidDateRange",
            $"The report period start date {start} cannot be later than the end date {end}.");
    }

    #endregion

    #endregion

    #region Check

    public static class BalanceCheck
    {
        public static readonly Func<Guid, decimal, decimal, Error> InsufficientFunds = (accountId, balance, amount) => new Error(
            "BalanceCheck.InsufficientFunds",
            $"Insufficient funds for account {accountId}. Balance: {balance}, Attempted Debit: {amount}");
    }

    public static class FraudCheck
    {
        public static readonly Func<Guid, decimal, Error> TransactionFlagged = (accountId, amount) => new Error(
            "FraudCheck.TransactionFlagged",
            $"High-value transaction flagged for account {accountId}. Amount: {amount}");
    }

    public static class AccountStatusCheck
    {
        public static readonly Func<Guid, Error> AccountInactive = accountId => new Error(
            "AccountStatusCheck.AccountInactive",
            $"Account {accountId} is inactive.");
    }

    #endregion

    #region Sms

    public static class Sms
    {
        public static readonly Func<string, string, Error> TwilioSendFailed = (phoneNumber, message) => new Error(
            "Sms.TwilioSendFailed",
            $"Failed to send SMS to {phoneNumber}: {message}");

        public static readonly Func<string, Error> GatewaySendFailed = (message) => new Error(
            "Sms.GatewaySendFailed",
            $"Failed to send SMS via gateway: {message}");
    }

    #endregion
}