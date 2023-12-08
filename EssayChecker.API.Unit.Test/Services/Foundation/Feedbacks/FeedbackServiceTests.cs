using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Services.Foundation.Feedbacks;
using Moq;
using Tynamix.ObjectFiller;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IFeedbackService feedbackService;
    public FeedbackServiceTests()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();
        this.feedbackService = new FeedbackService(
            storageBroker: storageBrokerMock.Object,
            loggingBroker: loggingBrokerMock.Object);
    }

    private static Feedback CreateRandomFeedback() =>
        CreateFeedbackFiller().Create();

    private static Filler<Feedback> CreateFeedbackFiller() =>
        new Filler<Feedback>();
}