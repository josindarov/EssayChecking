using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace EssayChecker.API.Services.Foundation.Users;

public partial class UserService
{
    private delegate ValueTask<User> ReturningUserFunction();

    private delegate IQueryable<User> ReturningUsersFunction();

    private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
    {
        try
        {
            return await returningUserFunction();
        }
        catch (UserNullException userNullException)
        {
            throw CreateAndLogValidationException(userNullException);
        }
        catch (InvalidUserException invalidUserException)
        {
            throw CreateAndLogValidationException(invalidUserException);
        }
        catch (NotFoundUserException notFoundUserException)
        {
            throw CreateAndLogValidationException(notFoundUserException);
        }
        catch (SqlException sqlException)
        {
            var userStorageException = new FailedUserStorageException(sqlException);
            throw CreateAndLogCriticalDependencyException(userStorageException);
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsUserException =
                new AlreadyExistsUserException(duplicateKeyException);

            throw CreateAndLogDependencyValidationException(alreadyExistsUserException);
        }
        catch (Exception exception)
        {
            var failedUserServiceException =
                new FailedUserServiceException(exception);

            throw CreateAndLogServiceException(failedUserServiceException);
        }
    }
    
    private IQueryable<User> TryCatch(ReturningUsersFunction returningUsersFunction)
    {
        try
        {
            return returningUsersFunction();
        }
        catch (SqlException sqlException)
        {
            var failedUserStorageException =
                new FailedUserStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedUserStorageException);
        }
        catch (Exception serviceException)
        {
            var failedServiceUserException =
                new FailedUserServiceException(serviceException);

            throw CreateAndLogServiceException(failedServiceUserException);
        }
    }

    private Exception CreateAndLogServiceException(Exception exception)
    {
        UserServiceException userServiceException = 
            new UserServiceException(exception);

        loggingBroker.LogError(userServiceException);
        return userServiceException;
    }

    private UserDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
    {
        UserDependencyValidationException userDependencyValidationException =
            new UserDependencyValidationException(exception);

        loggingBroker.LogError(userDependencyValidationException);
        return userDependencyValidationException;
    }
    private UserValidationException CreateAndLogValidationException(Xeption exception)
    {
        var userValidationException = 
            new UserValidationException(exception);

        this.loggingBroker.LogError(userValidationException);
        return userValidationException;
    }

    private UserDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var userDependencyException = 
            new UserDependencyException(exception);
        
        this.loggingBroker.LogCritical(userDependencyException);
        return userDependencyException;
    }
    
    
}