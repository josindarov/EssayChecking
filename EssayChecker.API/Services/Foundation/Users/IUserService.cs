using System.Linq;
using System.Threading.Tasks;

namespace EssayChecker.API.Services.Foundation.Users;

public interface IUserService
{
    ValueTask<Models.Foundation.Users.User> AddUserAsync(Models.Foundation.Users.User user);
    ValueTask<Models.Foundation.Users.User> RetrieveUserByIdAsync(Models.Foundation.Users.User user);
    IQueryable<Models.Foundation.Users.User> RetrieveAllUsers();
    ValueTask<Models.Foundation.Users.User> ModifyUserAsync(Models.Foundation.Users.User user);
    ValueTask<Models.Foundation.Users.User> RemoveUserAsync(Models.Foundation.Users.User user);
}