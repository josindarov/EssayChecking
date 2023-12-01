using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class UserDependencyException : Xeption
{
    public UserDependencyException(Exception innerException)
        : base("User dependency error occured, contact support", innerException)
    { }
}