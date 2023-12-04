using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<User> Users { get; set; }

    public async ValueTask<User> InsertUserAsync(User user) =>
        await InsertAsync(user);

    public async ValueTask<User> SelectUserByIdAsync(Guid id) =>
        await SelectAsync<User>(id);
    public IQueryable<User> SelectAllUsers() =>
        SelectAll<User>();

    public async ValueTask<User> UpdateUserAsync(User user) =>
        await UpdateAsync(user);

    public async ValueTask<User> DeleteUserAsync(User user) =>
        await DeleteAsync(user);
}