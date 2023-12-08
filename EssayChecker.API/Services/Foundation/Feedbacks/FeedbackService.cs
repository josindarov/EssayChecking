using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Feedbacks;

namespace EssayChecker.API.Services.Foundation.Feedbacks;

public class FeedbackService : IFeedbackService
{
    private readonly IStorageBroker storageBroker;
    private readonly ILoggingBroker loggingBroker;

    public FeedbackService(IStorageBroker storageBroker, 
        ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }
    public async ValueTask<Feedback> AddFeedbackAsync(Feedback feedback)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Feedback> RetrieveAllFeedbacks()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Feedback> RetrieveFeedbackByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}