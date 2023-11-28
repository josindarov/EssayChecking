using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;

namespace EssayChecker.API.Services.Foundation.Users;

public class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;

    public UserService(IStorageBroker storageBroker)
    {
        this.storageBroker = storageBroker;
    }
    public async ValueTask<User> AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> RetrieveUserByIdAsync(User user)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Models.Foundation.Users.User> RetrieveAllUsers()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> ModifyUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> RemoveUserAsync(User user)
    {
        throw new NotImplementedException();
    }
}