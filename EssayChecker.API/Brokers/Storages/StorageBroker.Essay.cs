using EssayChecker.API.Models.Foundation.Essays;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Essay> Essays { get; set; }
    
}