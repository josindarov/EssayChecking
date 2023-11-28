using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Storages;

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
        throw new NotImplementedException();
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