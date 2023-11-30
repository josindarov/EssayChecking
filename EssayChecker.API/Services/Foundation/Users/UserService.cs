using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EssayChecker.API.Services.Foundation.Users;

public partial class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;

    public UserService(IStorageBroker storageBroker)
    {
        this.storageBroker = storageBroker;
    }

    public ValueTask<User> AddUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnAdd(user);
            return await storageBroker.InsertUserAsync(user);
        });
    

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