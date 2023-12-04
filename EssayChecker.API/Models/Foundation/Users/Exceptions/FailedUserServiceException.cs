using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class FailedUserServiceException : Xeption
{
    public FailedUserServiceException(Exception innerException)
     : base("Failed user service error occured, contact support",
         innerException)
    { }
}