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

    public static class Teacher
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Teacher.NotFound",
            $"The teacher with the identifier {id} was not found.");

        public static Error NotAssignedToClass(Guid teacherId, Guid classId) => new(
            "Teacher.NotAssignedToClass",
            $"Teacher with ID {teacherId} is not assigned to class {classId}.");
    }

    public static class DepartmentHead
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "DepartmentHead.NotFound",
            $"The department head with the identifier {id} was not found.");
    }

    public static class Student
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Student.NotFound",
            $"The student with the identifier {id} was not found.");
    }

    #endregion

    #endregion
}