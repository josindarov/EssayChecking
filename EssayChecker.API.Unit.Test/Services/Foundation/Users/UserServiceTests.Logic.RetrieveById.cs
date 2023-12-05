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
    public async Task ShouldRetrieveUserById()
    {
        // given
        User randomUser = CreateRandomUser();
        User persistedUser = randomUser;
        User expectedUser = persistedUser.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectUserByIdAsync(randomUser.Id)).ReturnsAsync(persistedUser);
        
        // when
        User actualUser = await this.userService
            .RetrieveUserByIdAsync(randomUser.Id);
        
        // then
        actualUser.Should().BeEquivalentTo(expectedUser);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectUserByIdAsync(randomUser.Id),Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}