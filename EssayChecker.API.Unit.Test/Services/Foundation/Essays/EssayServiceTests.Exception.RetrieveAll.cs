using System;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
    {
        // given
        SqlException sqlException = GetSqlException();

        var failedEssayStorageException =
            new FailedEssayStorageException(sqlException);

        var expectedEssayDependencyException =
            new EssayDependencyException(failedEssayStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllEssays())
            .Throws(sqlException);

        // when
        Action retrieveAllEssaysAction = () =>
            this.essayService.RetrieveAllEssays();

        EssayDependencyException actuaEssayDependencyException = 
            Assert.Throws<EssayDependencyException>(retrieveAllEssaysAction);

        // then
        actuaEssayDependencyException.Should().BeEquivalentTo(
            expectedEssayDependencyException);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllEssays(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedEssayDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}