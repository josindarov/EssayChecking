using Xeptions;

namespace EssayChecker.API.Models.Foundation.Users.Exceptions;

public class UserNullException : Xeption
{
    public UserNullException()
        : base(message:"User is null.")
    {}
}