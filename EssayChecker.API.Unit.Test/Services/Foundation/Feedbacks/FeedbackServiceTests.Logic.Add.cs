using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    [Fact]
    public async Task ShouldFeedbackAddAsync()
    {
        // given
        Feedback randomFeedback = CreateRandomFeedback();
        Feedback inputFeedback = randomFeedback;
        Feedback persistedFeedback = inputFeedback;
        Feedback expectedFeedback = persistedFeedback.DeepClone();

        storageBrokerMock.Setup(broker =>
            broker.InsertFeedbackAsync(inputFeedback))
            .ReturnsAsync(persistedFeedback);
        
        // when
        Feedback actualFeedback = await this.feedbackService
            .AddFeedbackAsync(inputFeedback);

        // then
        actualFeedback.Should().BeEquivalentTo(expectedFeedback);
        
        storageBrokerMock.Verify(broker => 
            broker.InsertFeedbackAsync(inputFeedback),
            Times.Once);
        
        storageBrokerMock.VerifyNoOtherCalls();
        loggingBrokerMock.VerifyNoOtherCalls();
    }
}