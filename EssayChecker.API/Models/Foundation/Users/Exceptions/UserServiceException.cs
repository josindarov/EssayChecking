using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class UserServiceException : Xeption
{
    public UserServiceException(Xeption innerException)
        :base("User service error occured, contact support", innerException) 
    {}
}