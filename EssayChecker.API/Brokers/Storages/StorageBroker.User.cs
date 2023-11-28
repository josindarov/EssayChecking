using EssayChecker.API.Models.Foundation.Users;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<User> Users { get; set; }
}