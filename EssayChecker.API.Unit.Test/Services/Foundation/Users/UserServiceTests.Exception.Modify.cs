using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        SqlException sqlException = GetSqlException();

        var failedUserStorageException =
            new FailedUserStorageException(sqlException);

        var expectedUserDependencyException = 
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(randomUser.Id)).ReturnsAsync(randomUser);

        this.storageBrokerMock.Setup(broker =>
            broker.UpdateUserAsync(randomUser)).ThrowsAsync(sqlException);
        
        // when
        ValueTask<User> modifyUserTask = 
            this.userService.ModifyUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(
                modifyUserTask.AsTask);
        
        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(randomUser.Id),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(randomUser),
            Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowServiceExceptionOnUpdateIfServiceErrorOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        var serviceException = new Exception();

        var failedUserServiceException = 
            new FailedUserServiceException(serviceException);

        UserServiceException expectedUserServiceException =
            new UserServiceException(failedUserServiceException);

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(randomUser.Id)).ReturnsAsync(randomUser);

        this.storageBrokerMock.Setup(broker =>
            broker.UpdateUserAsync(randomUser)).ThrowsAsync(serviceException);
        
        // when
        ValueTask<User> updateUserTask = this.userService.ModifyUserAsync(randomUser);

        UserServiceException actualUserServiceException =
            await Assert.ThrowsAsync<UserServiceException>(updateUserTask.AsTask);
        
        // then
        actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(randomUser.Id),Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.UpdateUserAsync(randomUser),Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedUserServiceException))),
            Times.Once());
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}