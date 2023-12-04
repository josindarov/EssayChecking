using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class UserDependencyValidationException : Xeption
{
    public UserDependencyValidationException(Xeption exception)
        : base("User dependency validation error occured, fix the error and try again")
    { }
}