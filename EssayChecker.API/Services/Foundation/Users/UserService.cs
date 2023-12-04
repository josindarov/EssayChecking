using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EssayChecker.API.Services.Foundation.Users;

public partial class UserService : IUserService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;

    public UserService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<User> AddUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnAdd(user);
            return await storageBroker.InsertUserAsync(user);
        });
    

    public async ValueTask<User> RetrieveUserByIdAsync(Guid id)
    {
        return await this.storageBroker.SelectUserByIdAsync(id);
    }

    public IQueryable<User> RetrieveAllUsers()
    {
        throw new System.NotImplementedException();
    }

    public async ValueTask<User> ModifyUserAsync(User user)
    {
        User updatedUser = await RetrieveUserByIdAsync(user.Id);
        return await this.storageBroker.UpdateUserAsync(updatedUser);
    }

    public async ValueTask<User> RemoveUserAsync(Guid id)
    {
        throw new System.NotImplementedException();
    }
}