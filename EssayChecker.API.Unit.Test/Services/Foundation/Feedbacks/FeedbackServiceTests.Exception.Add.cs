using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Feedback randomFeedback = CreateRandomFeedback();
        SqlException sqlException = GetSqlException();

        var failedFeedbackStorageException =
            new FailedFeedbackStorageException(sqlException);

        var expectedFeedbackDependencyException =
            new FeedbackDependencyException(failedFeedbackStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertFeedbackAsync(randomFeedback)).ThrowsAsync(sqlException);
        
        // when
        ValueTask<Feedback> addFeedbackTask = this.feedbackService.AddFeedbackAsync(randomFeedback);

        var actualFeedbackDependencyException =
            await Assert.ThrowsAsync<FeedbackDependencyException>(addFeedbackTask.AsTask);

        // then
        actualFeedbackDependencyException.Should().BeEquivalentTo(expectedFeedbackDependencyException);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedFeedbackDependencyException))),Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertFeedbackAsync(randomFeedback),Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}