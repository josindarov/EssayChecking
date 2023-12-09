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
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedFeedbackValidationException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnAddIfFeedbackIsInvalidAndLogItAsync(
        string invalidText)
    {
        // given 
        Feedback invalidFeedback = new Feedback()
        {
            Comment = invalidText
        };

        var invalidFeedbackException = new InvalidFeedbackException();
        
        invalidFeedbackException.AddData(
            key: nameof(Feedback.Id),
            values: "Id is required");
        
        invalidFeedbackException.AddData(
            key: nameof(Feedback.EssayId),
            values: "Id is required");
        
        invalidFeedbackException.AddData(
            key: nameof(Feedback.Comment),
            values: "Text is required");
        
        var expectedFeedbackValidationException =
            new FeedbackValidationException(invalidFeedbackException);
        
        // when
        ValueTask<Feedback> addFeedbackTask = this.feedbackService.AddFeedbackAsync(invalidFeedback);

        var actualFeedbackValidationException =
            await Assert.ThrowsAsync<FeedbackValidationException>(addFeedbackTask.AsTask);

        // then
        actualFeedbackValidationException.Should().BeEquivalentTo(expectedFeedbackValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedFeedbackValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertFeedbackAsync(invalidFeedback),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}