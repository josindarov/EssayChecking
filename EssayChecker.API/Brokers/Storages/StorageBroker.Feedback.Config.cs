using EssayChecker.API.Models.Foundation.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker
{
    private static void AddFeedbackConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feedback>()
            .HasOne(feedback => feedback.Essay)
            .WithOne(essay => essay.Feedback)
            .OnDelete(DeleteBehavior.Cascade);
    }
}