using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.DateTimes;
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
    private readonly IDateTimeBroker dateTimeBroker;

    public UserService(IStorageBroker storageBroker, ILoggingBroker loggingBroker, IDateTimeBroker dateTimeBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
        this.dateTimeBroker = dateTimeBroker;
    }

    public ValueTask<User> AddUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnAdd(user);
            return await storageBroker.InsertUserAsync(user);
        });


    public ValueTask<User> RetrieveUserByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            User user = await this.storageBroker.SelectUserByIdAsync(id);
            ValidateStorageUser(user, id);
            return user;
        });

    public IQueryable<User> RetrieveAllUsers()
    { 
        return this.storageBroker.SelectAllUsers();
    }

    public ValueTask<User> ModifyUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);
            
            User updatedUser = await this.storageBroker
                .SelectUserByIdAsync(user.Id);
            
            ValidateStorageUser(updatedUser, user.Id);
            
            return await this.storageBroker
                .UpdateUserAsync(updatedUser);
        });

    public ValueTask<User> RemoveUserAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            User deletedUser = await this.storageBroker.SelectUserByIdAsync(id);
            ValidateStorageUser(deletedUser, id);

            return await this.storageBroker
                .DeleteUserAsync(deletedUser);
        });
}