using EssayChecker.API.Models.Foundation.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker
{
    public DbSet<Feedback> Feedbacks { get; set; }
}