using System;
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
    public async Task ShouldRetrieveFeedbackById()
    {
        // given
        Feedback randomFeedback = CreateRandomFeedback();
        Feedback persistedFeedback = randomFeedback;
        Feedback expectedFeedback = persistedFeedback.DeepClone();

        this.storageBrokerMock.Setup(broker => broker.SelectFeedbackByIdAsync(randomFeedback.Id))
            .ReturnsAsync(persistedFeedback);
        
        // when
        Feedback actualFeedback = await this.feedbackService.RetrieveFeedbackByIdAsync(randomFeedback.Id);
        
        // then
        actualFeedback.Should().BeEquivalentTo(expectedFeedback);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectFeedbackByIdAsync(randomFeedback.Id),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}