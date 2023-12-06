using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using Xeptions;

namespace EssayChecker.API.Services.Foundation.Essays;

public partial class EssayService
{
    private delegate ValueTask<Essay> ReturningUserFunction();

    private async ValueTask<Essay> TryCatch(ReturningUserFunction returningUserFunction)
    {
        try
        {
            return await returningUserFunction();
        }
        catch (EssayNullException essayNullException)
        {
            throw CreateAndLogValidationException(essayNullException);
        }
        catch (InvalidEssayException invalidEssayException)
        {
            throw CreateAndLogValidationException(invalidEssayException);
        }
    }

    private EssayValidationException CreateAndLogValidationException(Xeption exception)
    {
        var essayValidationException = new EssayValidationException(exception);
        this.loggingBroker.LogError(essayValidationException);
        return essayValidationException;
    }
}