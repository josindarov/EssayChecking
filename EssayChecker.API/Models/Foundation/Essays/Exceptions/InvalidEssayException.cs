using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class InvalidEssayException : Xeption
{
    public InvalidEssayException()
        :base("User is invalid")
    { }
}