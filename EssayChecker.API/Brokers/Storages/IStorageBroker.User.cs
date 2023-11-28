using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;

namespace EssayChecker.API.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<User> InsertUserAsync(User user);
    ValueTask<User> SelectUserByIdAsync(User user);
    IQueryable<User> SelectAllUsers();
    ValueTask<User> UpdateUserAsync(User user);
    ValueTask<User> DeleteUserAsync(User user);
}