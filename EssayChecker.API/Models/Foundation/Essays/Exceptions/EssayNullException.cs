using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class EssayNullException : Xeption
{
    public EssayNullException()
        :base("Essay is null")
    { }
}