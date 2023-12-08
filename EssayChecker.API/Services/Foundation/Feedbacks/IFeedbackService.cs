using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;

namespace EssayChecker.API.Services.Foundation.Feedbacks;

public interface IFeedbackService
{
    ValueTask<Feedback> AddFeedbackAsync(Feedback feedback);
    IQueryable<Feedback> RetrieveAllFeedbacks();
    ValueTask<Feedback> RetrieveFeedbackByIdAsync(Guid id);
}