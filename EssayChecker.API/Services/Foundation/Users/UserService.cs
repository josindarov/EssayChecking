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
        try
        {
            if (user is null)
            {
                throw new UserNullException();
            }
            User updatedUser = await RetrieveUserByIdAsync(user.Id);
            return await this.storageBroker.UpdateUserAsync(updatedUser);
        }
        catch (UserNullException userNullException)
        {
            var userValidationException = 
                new UserValidationException(userNullException);

            this.loggingBroker.LogError(userValidationException);
            throw userValidationException;
        }
        
    }

    public async ValueTask<User> RemoveUserAsync(Guid id)
    {
        throw new System.NotImplementedException();
    }
}