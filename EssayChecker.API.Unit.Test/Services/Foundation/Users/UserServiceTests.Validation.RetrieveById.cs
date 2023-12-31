using System;
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
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidUserId = Guid.Empty;
        var invalidUserException = new InvalidUserException();
        
        invalidUserException.AddData(
            key:nameof(User.Id),
            values:"Id is required");

        var expectedUserValidationException = 
            new UserValidationException(invalidUserException);
        
        // when
        ValueTask<User> retrieveUserByIdTask = 
            this.userService.RetrieveUserByIdAsync(invalidUserId);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(retrieveUserByIdTask.AsTask);
        
        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
            Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfUserIsNotFoundAndLogItAsync()
    {
        // given
        Guid someUserId = Guid.NewGuid();
        User noUser = null;
        var notFoundUserException = new NotFoundUserException(someUserId);

        var expectedUserValidationException = 
            new UserValidationException(notFoundUserException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noUser);
        
        // when
        ValueTask<User> retrieveUserByIdTask = this.userService.RetrieveUserByIdAsync(someUserId);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(retrieveUserByIdTask.AsTask);

        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}