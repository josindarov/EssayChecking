using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using InvalidUserException = EssayChecker.API.Models.Foundation.Users.Exceptions.InvalidUserException;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidId = Guid.Empty;
        var invalidUserException = new InvalidUserException();

        invalidUserException.AddData(
            key:nameof(User.Id),
            values: "Id is required");
        
        var expectedUserDependencyException = 
            new UserValidationException(invalidUserException);
        
        // when
        ValueTask<User> removeUserById = this.userService.RemoveUserAsync(invalidId);

        UserValidationException actualUserDependencyException =
            await Assert.ThrowsAsync<UserValidationException>(removeUserById.AsTask);

        // then
        actualUserDependencyException.Should().BeEquivalentTo(expectedUserDependencyException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(
                expectedUserDependencyException))), Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>()),Times.Never);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(It.IsAny<User>()),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
    
}