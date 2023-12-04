using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class UserServiceException : Xeption
{
    public UserServiceException(Exception innerException)
        :base("User service exception occured, fix it and try again.",
            innerException)
    { }
}