using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;

namespace EssayChecker.API.Services.Foundation.Users;

public interface IUserService
{
    ValueTask<Models.Foundation.Users.User> AddUserAsync(User user);
    ValueTask<Models.Foundation.Users.User> RetrieveUserByIdAsync(Guid id);
    IQueryable<Models.Foundation.Users.User> RetrieveAllUsers();
    ValueTask<Models.Foundation.Users.User> ModifyUserAsync(User user);
    ValueTask<Models.Foundation.Users.User> RemoveUserAsync(Guid id);
}