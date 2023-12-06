using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Essay randomEssay = CreateRandomEssay();
        SqlException sqlException = GetSqlException();

        var failedEssayStorageException = 
            new FailedEssayStorageException(sqlException);

        var expectedEssayDependencyException = 
            new EssayDependencyException(failedEssayStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertEssayAsync(randomEssay)).Throws(sqlException);
        
        // when
        ValueTask<Essay> addEssayTask = this.essayService.InsertEssayAsync(randomEssay);

        var actualEssayDependencyException =
            await Assert.ThrowsAsync<EssayDependencyException>(addEssayTask.AsTask);

        // then
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayDependencyException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogCritical(It.Is(SameExceptionAs(expectedEssayDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertEssayAsync(randomEssay),Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
        
    }
    
    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogIdAsync()
    {
        // given
        Essay randomEssay = CreateRandomEssay();
        var serviceException = new Exception();
        var failedEssayServiceException = new FailedEssayServiceException(serviceException);

        var expectedEssayServiceException = new EssayServiceException(failedEssayServiceException);

        storageBrokerMock.Setup(broker =>
                broker.InsertEssayAsync(randomEssay))
            .ThrowsAsync(serviceException);
        // when
        ValueTask<Essay> addEssayTask = this.essayService.InsertEssayAsync(randomEssay);

        EssayServiceException actualEssayServiceException =
            await Assert.ThrowsAsync<EssayServiceException>(addEssayTask.AsTask);

        // then
        actualEssayServiceException.Should().BeEquivalentTo(expectedEssayServiceException);

        loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedEssayServiceException))), Times.Once);

        storageBrokerMock.Verify(broker =>
            broker.InsertEssayAsync(randomEssay), Times.Once);

        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
    }
}