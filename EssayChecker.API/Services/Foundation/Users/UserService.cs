using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users.Exceptions;

namespace EssayChecker.API.Services.Foundation.Users;

public class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;

    public UserService(IStorageBroker storageBroker)
    {
        this.storageBroker = storageBroker;
    }
    public async ValueTask<Models.Foundation.Users.User> AddUserAsync(Models.Foundation.Users.User user)
    {
        try
        {
            if (user is null)
            {
                throw new UserNullException();
            }
            return await storageBroker.InsertUserAsync(user);
        }
        catch (UserNullException userNullException)
        {
            var userValidationException = 
                new UserValidationException(userNullException);
            
            throw userValidationException;
        }
    }

    public async ValueTask<Models.Foundation.Users.User> RetrieveUserByIdAsync(Models.Foundation.Users.User user)
    {
        throw new System.NotImplementedException();
    }

    public IQueryable<Models.Foundation.Users.User> RetrieveAllUsers()
    {
        throw new System.NotImplementedException();
    }

    public async ValueTask<Models.Foundation.Users.User> ModifyUserAsync(Models.Foundation.Users.User user)
    {
        throw new System.NotImplementedException();
    }

    public async ValueTask<Models.Foundation.Users.User> RemoveUserAsync(Models.Foundation.Users.User user)
    {
        throw new System.NotImplementedException();
    }
}