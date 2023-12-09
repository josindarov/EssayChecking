using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidFeedbackId = Guid.Empty;
        var invalidFeedbackException = new InvalidFeedbackException();
        
        invalidFeedbackException.AddData(
            key:nameof(Feedback.Id),
            values:"Id is required");

        var expectedFeedbackValidationException = 
            new FeedbackValidationException(invalidFeedbackException);
        
        // when
        ValueTask<Feedback> retrieveFeedbackByIdTask = 
            this.feedbackService.RetrieveFeedbackByIdAsync(invalidFeedbackId);

        FeedbackValidationException actualFeedbackValidationException =
            await Assert.ThrowsAsync<FeedbackValidationException>(retrieveFeedbackByIdTask.AsTask);
        
        // then
        actualFeedbackValidationException.Should().BeEquivalentTo(expectedFeedbackValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedFeedbackValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectEssayByIdAsync(It.IsAny<Guid>()),
            Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfFeedbackIsNotFoundAndLogItAsync()
    {
        // given
        Guid someFeedbackId = Guid.NewGuid();
        Feedback noFeedback = null;
        var notFoundFeedbackException = new NotFoundFeedbackException(someFeedbackId);

        var expectedFeedbackValidationException = 
            new FeedbackValidationException(notFoundFeedbackException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectFeedbackByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(noFeedback);
        
        // when
        ValueTask<Feedback> retrieveFeedbackByIdTask = this.feedbackService.RetrieveFeedbackByIdAsync(someFeedbackId);

        FeedbackValidationException actualFeedbackValidationException =
            await Assert.ThrowsAsync<FeedbackValidationException>(retrieveFeedbackByIdTask.AsTask);

        // then
        actualFeedbackValidationException.Should().BeEquivalentTo(expectedFeedbackValidationException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectFeedbackByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedFeedbackValidationException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}