using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionIfFeedbackIsNullAndLogItAsync()
    {
        // given
        Feedback nullFeedback = null;
        var feedbackNullException = new FeedbackNullException();
        var expectedFeedbackValidationException =
            new FeedbackValidationException(feedbackNullException);
        
        // when
        ValueTask<Feedback> addFeedbackTask = feedbackService.AddFeedbackAsync(nullFeedback);

        FeedbackValidationException actualFeedbackValidationException =
            await Assert.ThrowsAsync<FeedbackValidationException>(addFeedbackTask.AsTask);

        // then
        actualFeedbackValidationException.Should().BeEquivalentTo(expectedFeedbackValidationException);
        
        loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedFeedbackValidationException))),
            Times.Never);
        
        storageBrokerMock.VerifyNoOtherCalls();
        loggingBrokerMock.VerifyNoOtherCalls();
    }
}