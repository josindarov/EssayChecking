using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class EssayValidationException : Xeption
{
    public EssayValidationException(Xeption innerException)
        :base("Validation error occured, fix it and try again.", 
            innerException)
    { }
}