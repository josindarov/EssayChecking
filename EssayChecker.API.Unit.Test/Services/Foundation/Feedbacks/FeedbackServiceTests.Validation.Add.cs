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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnAddIfFeedbackIsInvalidAndLogItAsync(
        string invalidText)
    {
        // given
        Feedback someFeedback = new Feedback()
        {
            Comment = invalidText
        };

        var invalidFeedbackException = new InvalidFeedbackException();
        
        invalidFeedbackException.AddData(
            key:nameof(Feedback.Id),
            values:"Id is required");
        
        invalidFeedbackException.AddData(
            key: nameof(Feedback.Comment),
            values: "Text is required");
        
        invalidFeedbackException.AddData(
            key: nameof(Feedback.Mark),
            values: "Mark is required");
        
        invalidFeedbackException.AddData(
            key: nameof(Feedback.EssayId),
            values: "Id is required");

        var expectedFeedbackValidationException =
            new FeedbackValidationException(invalidFeedbackException);
        
        // when
        ValueTask<Feedback> addFeedbackTask = feedbackService.AddFeedbackAsync(someFeedback);

        var actualFeedbackValidationException =
            await Assert.ThrowsAsync<FeedbackValidationException>(addFeedbackTask.AsTask);
        
        // then
        actualFeedbackValidationException.Should().BeEquivalentTo(expectedFeedbackValidationException);
        
        loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedFeedbackValidationException))),
            Times.Once);
        
        storageBrokerMock.Verify(broker => 
            broker.InsertFeedbackAsync(someFeedback),Times.Never);
        
        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
    }
}