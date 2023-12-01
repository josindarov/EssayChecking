using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class FailedUserStorageException : Xeption
{
    public FailedUserStorageException(Exception innerException)
        : base("Failed user storage error occured, contact support.", innerException)
    { }
}