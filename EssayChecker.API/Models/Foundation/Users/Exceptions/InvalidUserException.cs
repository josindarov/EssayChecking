using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class InvalidUserException : Xeption
{
    public InvalidUserException()
        : base("User is invalid.")
    { }
}