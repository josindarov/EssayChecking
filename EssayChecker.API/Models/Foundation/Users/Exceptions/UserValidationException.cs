using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class UserValidationException : Xeption
{
    public UserValidationException(Xeption innerException)
        : base("User validation error occured. Fix the error and try again.",
            innerException)
    { }
    
}