using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
    {
        // given
        Guid someEssayId = Guid.NewGuid();
        SqlException sqlException = GetSqlException();
        var failedEssayStorageException = new FailedEssayStorageException(sqlException);
        
        EssayDependencyException expectedEssayDependencyException =
            new EssayDependencyException(failedEssayStorageException);
        
        this.storageBrokerMock.Setup(broker =>
                broker.SelectEssayByIdAsync(It.IsAny<Guid>()))
            .ThrowsAsync(sqlException);
        
        // when
        ValueTask<Essay> retrieveEssayByIdTask = this.essayService.RetrieveEssayById(someEssayId);

        EssayDependencyException actualEssayDependencyException =
            await Assert.ThrowsAsync<EssayDependencyException>(retrieveEssayByIdTask.AsTask);

        // then
        actualEssayDependencyException.Should().BeEquivalentTo(expectedEssayDependencyException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedEssayDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}