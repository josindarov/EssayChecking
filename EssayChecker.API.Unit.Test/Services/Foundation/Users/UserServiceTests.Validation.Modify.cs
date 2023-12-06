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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task ShouldThrowValidationExceptionOnUpdateIfUserIsInvalidAndLogItAsync(
        string invalidText)
    {
        // given
        var invalidUser = new User()
        {
            Name = invalidText,
            TelephoneNumber = invalidText
        };

        InvalidUserException invalidUserException = new InvalidUserException();
        
        invalidUserException.AddData(
            key: nameof(User.Id),
            values:"Id is required");
        
        invalidUserException.AddData(
            key: nameof(User.Name),
            values: "Text is required");
        
        invalidUserException.AddData(
            key: nameof(User.TelephoneNumber),
            values: "Text is required");

        var expectedUserValidationException = 
            new UserValidationException(invalidUserException);
        
        // when
        ValueTask<User> updateUserTask = this.userService.ModifyUserAsync(invalidUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(updateUserTask.AsTask);
        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedUserValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedUserValidationException)))
            ,Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.UpdateUserAsync(invalidUser),Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}