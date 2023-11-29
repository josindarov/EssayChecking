using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
using FluentAssertions;
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
}