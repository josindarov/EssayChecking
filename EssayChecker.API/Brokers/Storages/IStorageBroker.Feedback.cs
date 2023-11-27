using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;

namespace EssayChecker.API.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Feedback> InsertFeedbackAsync(Feedback feedback);
    ValueTask<Feedback> SelectFeedbackByIdAsync(Guid id);
    IQueryable<Feedback> SelectAllFeedbacks();
}