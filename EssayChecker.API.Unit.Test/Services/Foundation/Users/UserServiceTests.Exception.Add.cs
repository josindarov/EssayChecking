using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
    public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        SqlException sqlException = GetSqlException();
        
        var failedUserStorageException = 
            new FailedUserStorageException(sqlException);
        
        var expectedUserDependencyException = 
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(randomUser)).ThrowsAsync(sqlException);
        
        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(randomUser);

        UserDependencyException actualUserDependencyException =
            await Assert.ThrowsAsync<UserDependencyException>(addUserTask.AsTask);

        // then
        actualUserDependencyException.Should()
            .BeEquivalentTo(expectedUserDependencyException);
        
        storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(randomUser),Times.Once);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedUserDependencyException))), 
            Times.Once);
        
        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
        
    }

    [Fact]
    public async Task ShouldThrowDependencyValidationExceptionOnAddIfDuplicateKeyErrorOccuredAndLogItAsync()
    {
        // given 
        User randomUser = CreateRandomUser();
        string someMessage = GetRandomString();
        var duplicateKeyException = new DuplicateKeyException(someMessage);

        var alreadyExistsUserException = 
            new AlreadyExistsUserException(duplicateKeyException);

        var expectedUserDependencyValidationException =
            new UserDependencyValidationException(alreadyExistsUserException);
        
        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(randomUser))
            .Throws(duplicateKeyException);
        
        this.storageBrokerMock.Setup(broker => broker.InsertUserAsync(randomUser))
            .Throws(duplicateKeyException);

        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(randomUser);
        
        UserDependencyValidationException actualUserDependencyValidationException =
            await Assert.ThrowsAsync<UserDependencyValidationException>(
                addUserTask.AsTask);
        
        // then
        actualUserDependencyValidationException.Should().BeEquivalentTo(expectedUserDependencyValidationException);
        
        
        loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                expectedUserDependencyValidationException))),Times.Once);
        
        storageBrokerMock.Verify(broker => 
            broker.InsertUserAsync(randomUser),
            Times.Once);
        
        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
    }   
    

    [Fact]
    public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccuredAndLogIdAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        var serviceException = new Exception();
        var failedUserServiceException = new FailedUserServiceException(serviceException);

        var expectedUserServiceException = 
            new UserServiceException(failedUserServiceException);

        storageBrokerMock.Setup(broker =>
                broker.InsertUserAsync(randomUser))
            .ThrowsAsync(serviceException);
        // when
        ValueTask<User> addUserTask = this.userService.AddUserAsync(randomUser);

        UserServiceException actualUserServiceException =
            await Assert.ThrowsAsync<UserServiceException>(addUserTask.AsTask);

        // then
        actualUserServiceException.Should().BeEquivalentTo(expectedUserServiceException);

        loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(
                expectedUserServiceException))), Times.Once);

        storageBrokerMock.Verify(broker =>
            broker.InsertUserAsync(randomUser), Times.Once);

        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
    }

    
}