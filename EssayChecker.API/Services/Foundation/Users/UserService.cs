using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;

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


    public ValueTask<User> RetrieveUserByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateUserId(id);
            User user = await this.storageBroker.SelectUserByIdAsync(id);
            ValidateStorageUser(user, id);
            return user;
        });

    public IQueryable<User> RetrieveAllUsers() =>
        TryCatch(() => this.storageBroker.SelectAllUsers());
    
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