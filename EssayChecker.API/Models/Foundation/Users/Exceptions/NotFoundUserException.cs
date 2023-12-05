using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class NotFoundUserException : Xeption
{
    public NotFoundUserException(Guid userId)
        :base($"User is not found in {userId} id.")
    { }
}