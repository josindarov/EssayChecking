using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using Microsoft.Data.SqlClient;
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
        catch (SqlException sqlException)
        {
            var failedEssayStorageException =
                new FailedEssayStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedEssayStorageException);
        }
        catch (Exception exception)
        {
            var failedEssayServiceException = 
                new FailedEssayServiceException(exception);

            throw CreateAndLogServiceException(failedEssayServiceException);
        }
    }

    private EssayServiceException CreateAndLogServiceException(Exception exception)
    {
        var essayServiceException = 
            new EssayServiceException(exception);

        this.loggingBroker.LogError(essayServiceException);
        return essayServiceException;
    }

    private Exception CreateAndLogCriticalDependencyException(Exception exception)
    {
        var essayDependencyException = 
            new EssayDependencyException(exception);
        
        this.loggingBroker.LogCritical(essayDependencyException);
        return essayDependencyException;
    }

    private EssayValidationException CreateAndLogValidationException(Xeption exception)
    {
        var essayValidationException = new EssayValidationException(exception);
        this.loggingBroker.LogError(essayValidationException);
        return essayValidationException;
    }
}