using System;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
    {
        // given
        SqlException sqlException = GetSqlException();

        var failedUserStorageException =
            new FailedUserStorageException(sqlException);

        var expectedUserDependencyException =
            new UserDependencyException(failedUserStorageException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
            .Throws(sqlException);

        // when
        Action retrieveAllUsersAction = () =>
            this.userService.RetrieveAllUsers();

        UserDependencyException actualUserDependencyException = 
            Assert.Throws<UserDependencyException>(retrieveAllUsersAction);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(
            expectedUserDependencyException);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
    
    [Fact]
    public void ShouldThrowServiceExceptionOnRetrieveAllWhenServiceErrorOccursAndLogIt()
    {
        //given
        string exceptionMessage = GetRandomString();
        var serviceException = new Exception(exceptionMessage);

        var failedUserServiceException =
            new FailedUserServiceException(serviceException);

        var expectedUserServiceException =
            new UserServiceException(failedUserServiceException);

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
            .Throws(serviceException);

        //when
        Action retrieveAllUsersAction = () =>
            this.userService.RetrieveAllUsers();

        UserServiceException actualUserServiceException = 
            Assert.Throws<UserServiceException>(retrieveAllUsersAction);

        //then
        actualUserServiceException.Should().BeEquivalentTo(
            expectedUserServiceException);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
            Times.Once);

        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserServiceException))),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }


}