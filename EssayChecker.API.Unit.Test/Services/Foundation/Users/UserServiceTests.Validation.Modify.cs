using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnUpdateIfUserIsNullAndLogItAsync()
    {
        // given 
        User nullUser = null;
        var userNullException = new UserNullException();

        var expectedUserValidationException =
            new UserValidationException(userNullException);
        
        // when 
        ValueTask<User> modifyUserTask = this.userService.ModifyUserAsync(nullUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(modifyUserTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                expectedUserValidationException))),Times.Once);
        
        this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
            Times.Never);

        this.storageBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()),
            Times.Never);
        
        loggingBrokerMock.VerifyNoOtherCalls();
        storageBrokerMock.VerifyNoOtherCalls();
    }
}