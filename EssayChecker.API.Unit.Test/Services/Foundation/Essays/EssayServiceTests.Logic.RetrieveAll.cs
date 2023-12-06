using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllEssays()
    {
        // given
        IQueryable<Essay> randomEssays = CreateRandomEssays();
        IQueryable<Essay> storageEssays = randomEssays;
        IQueryable<Essay> expectedEssays = storageEssays;

        this.storageBrokerMock.Setup(broker =>
                broker.SelectAllEssays())
            .Returns(storageEssays);

        // when
        IQueryable<Essay> actualEssays =
            this.essayService.RetrieveAllEssays();

        // then
        actualEssays.Should().BeEquivalentTo(expectedEssays);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectAllEssays(),
            Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}