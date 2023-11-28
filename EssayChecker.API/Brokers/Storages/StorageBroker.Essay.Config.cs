using EssayChecker.API.Models.Foundation.Essays;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker
{
    private static void AddEssayConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Essay>()
            .HasOne(essay => essay.User)
            .WithMany(user => user.Essays)
            .HasForeignKey(essay => essay.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}