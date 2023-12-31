using System;
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
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someFeedbackId = Guid.NewGuid();
        SqlException sqlException = GetSqlException();

        var failedFeedbackStorageException =
            new FailedFeedbackStorageException(sqlException);

        var expectedFeedbackDependencyException = 
            new FeedbackDependencyException(failedFeedbackStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectFeedbackByIdAsync(It.IsAny<Guid>())).ThrowsAsync(sqlException);
        
        // when
        ValueTask<Feedback> retrieveFeedbackByIdTask = this.feedbackService.RetrieveFeedbackByIdAsync(someFeedbackId);

        FeedbackDependencyException actualFeedbackDependencyException =
            await Assert.ThrowsAsync<FeedbackDependencyException>(retrieveFeedbackByIdTask.AsTask);
        
        // then
        actualFeedbackDependencyException.Should().BeEquivalentTo(expectedFeedbackDependencyException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectFeedbackByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(expectedFeedbackDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceExceptionOccursAndLogItAsync()
    {
        // given 
        Guid someFeedbackId = Guid.NewGuid();
        Exception serviceException = new Exception();

        var failedFeedbackServiceException =
            new FailedFeedbackServiceException(serviceException);

        var expectedFeedbackServiceException =
            new FeedbackServiceException(failedFeedbackServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectFeedbackByIdAsync(It.IsAny<Guid>())).ThrowsAsync(serviceException);

        // when 
        ValueTask<Feedback> retrieveFeedbackByIdTask = this.feedbackService
            .RetrieveFeedbackByIdAsync(someFeedbackId);

        FeedbackServiceException actualFeedbackServiceException =
            await Assert.ThrowsAsync<FeedbackServiceException>(retrieveFeedbackByIdTask.AsTask);

        // then
        actualFeedbackServiceException.Should().BeEquivalentTo(expectedFeedbackServiceException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectFeedbackByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedFeedbackServiceException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}