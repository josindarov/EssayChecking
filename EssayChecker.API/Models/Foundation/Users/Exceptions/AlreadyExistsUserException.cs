using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class AlreadyExistsUserException : Xeption
{
    public AlreadyExistsUserException(Exception innerException)
        : base("User already exists.", innerException)
    { }
}