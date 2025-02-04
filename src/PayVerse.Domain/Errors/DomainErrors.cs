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

        public static readonly Error NotExist = new(
            "AccountNumber.NotExist",
            $"There is no AccountNumber");
        
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
    
    #endregion
    
    #region Wallet
    
    #region Entities
    
    public static class Wallet
    {
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
}