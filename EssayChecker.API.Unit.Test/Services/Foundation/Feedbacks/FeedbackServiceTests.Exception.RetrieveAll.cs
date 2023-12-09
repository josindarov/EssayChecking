using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogItAsync()
    {
        // given
        SqlException sqlException = GetSqlException();

        var failedFeedbackStorageException =
            new FailedFeedbackStorageException(sqlException);

        var expectedFeedbackDependencyException =
            new FeedbackDependencyException(failedFeedbackStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFeedbacks())
            .Throws(sqlException);

        // when
        Action retrieveAllFeedbacksAction = () =>
            this.feedbackService.RetrieveAllFeedbacks();

        FeedbackDependencyException actualFeedbackDependencyException = 
            Assert.Throws<FeedbackDependencyException>(retrieveAllFeedbacksAction);

        // then
        actualFeedbackDependencyException.Should().BeEquivalentTo(
            expectedFeedbackDependencyException);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFeedbacks(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedFeedbackDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}