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
        
        loggingBrokerMock.Verify(broker =>
            broker.LogCritical(It.Is(SameExceptionAs(
                expectedUserDependencyException))), Times.Once());
        
        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
        
    }
}