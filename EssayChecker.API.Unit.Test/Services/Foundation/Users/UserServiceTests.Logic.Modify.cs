using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldUpdateUserAsync()
    {
        // given
        User randomUser = CreateRandomUser();
        User inputUser = randomUser;
        User storageUser = inputUser;
        User updatedUser = inputUser;
        User expectedUser = updatedUser.DeepClone();
        Guid inputUserId = updatedUser.Id;

        this.storageBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(inputUserId))
            .ReturnsAsync(storageUser);

        this.storageBrokerMock.Setup(broker =>
            broker.InsertUserAsync(inputUser)).ReturnsAsync(updatedUser);
        
        // when
        User actualUser = await this.userService.ModifyUserAsync(inputUser);

        // then
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(inputUserId),Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.UpdateUserAsync(inputUser),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
    
}