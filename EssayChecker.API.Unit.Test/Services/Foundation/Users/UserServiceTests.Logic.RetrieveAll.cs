using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Users;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users;

public partial class UserServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllUsers()
    {
        // given
        IQueryable<User> randomUsers = CreateRandomUsers();
        IQueryable<User> storageUsers = randomUsers;
        IQueryable<User> expectedUsers = storageUsers;

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllUsers())
            .Returns(storageUsers);

        // when
        IQueryable<User> actualUsers =
            this.userService.RetrieveAllUsers();

        // then
        actualUsers.Should().BeEquivalentTo(expectedUsers);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllUsers(),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}