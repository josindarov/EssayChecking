using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;

namespace EssayChecker.API.Services.Foundation.Users;

public partial class UserService
{
    private delegate ValueTask<User> ReturningUserFunction();

    private async ValueTask<User> TryCatch(ReturningUserFunction returningUserFunction)
    {
        try
        {
            return await returningUserFunction();
        }
        catch (UserNullException userNullException)
        {
            var userValidationException = 
                new UserValidationException(userNullException);
            
            throw userValidationException;
        }
    }
}