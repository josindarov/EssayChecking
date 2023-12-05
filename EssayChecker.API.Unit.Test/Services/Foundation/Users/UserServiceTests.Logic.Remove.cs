using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldUserRemoveById()
    {
        // given
        Guid randomId = Guid.NewGuid();
        Guid inputId = randomId;
        User randomUser = CreateRandomUser();
        User storagedUser = randomUser;
        User expectedInputUser = storagedUser;
        User deletedUser = expectedInputUser;
        User expectedUser = deletedUser.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(inputId))
            .ReturnsAsync(storagedUser);

        this.storageBrokerMock.Setup(broker =>
                broker.DeleteUserAsync(expectedInputUser))
            .ReturnsAsync(deletedUser);
        
        // when
        User actualUser = 
            await this.userService.RemoveUserAsync(inputId);
        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        this.storageBrokerMock.Verify(broker =>
            broker.SelectUserByIdAsync(inputId),Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.DeleteUserAsync(expectedInputUser),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}