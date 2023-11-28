using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker : IStorageBroker
{
    public DbSet<Feedback> Feedbacks { get; set; }

    public async ValueTask<Feedback> InsertFeedbackAsync(Feedback feedback) =>
        await InsertAsync(feedback);

    public async ValueTask<Feedback> SelectFeedbackByIdAsync(Guid id) =>
        await SelectAsync<Feedback>(id);

    public IQueryable<Feedback> SelectAllFeedbacks() =>
        SelectAll<Feedback>();
}