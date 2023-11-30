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
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAndLogItAsync()
    {
        // given
        User nullUser = null;
        var userNullException = new UserNullException();
        
        var expectedUserValidationException = 
            new UserValidationException(userNullException);

        // when
        ValueTask<User> addUserTask =
            this.userService.AddUserAsync(nullUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);
        
        // then
        actualUserValidationException.Should().BeEquivalentTo(
            expectedUserValidationException);
        
        storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsInvalidAndLogItAsync(
        string invalidText)
    {
        // given
        var invalidUser = new User()
        {
            Name = invalidText,
            TelephoneNumber = invalidText
        };

        var invalidUserException = new InvalidUserException();
        
        invalidUserException.AddData(
            key:nameof(User.Id),
            values:"Id is required");
        
        invalidUserException.AddData(
            key:nameof(User.Name),
            values:"Text is required");

        invalidUserException.AddData(
            key: nameof(User.TelephoneNumber),
            values: "Text is required");

        var expectedValidationException = 
            new UserValidationException(invalidUserException);
        
        // when
        ValueTask<User> addUserTask =
            this.userService.AddUserAsync(invalidUser);

        UserValidationException actualUserValidationException =
            await Assert.ThrowsAsync<UserValidationException>(addUserTask.AsTask);
        
        // then
        actualUserValidationException.Should().BeEquivalentTo(expectedValidationException);
        
        this.storageBrokerMock.Verify(broker =>
                broker.InsertUserAsync(invalidUser), Times.Never);
        
        storageBrokerMock.VerifyNoOtherCalls();
    }
    
}