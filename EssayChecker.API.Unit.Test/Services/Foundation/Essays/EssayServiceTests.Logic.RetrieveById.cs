using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public async Task ShouldEssayRetrieveById()
    {
        // given
        Essay randomEssay = CreateRandomEssay();
        Essay persistedEssay = randomEssay;
        Essay expectedEssay = persistedEssay.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.SelectEssayByIdAsync(randomEssay.Id)).ReturnsAsync(persistedEssay);
        
        // when
        Essay actualEssay = await this.essayService
            .RetrieveEssayById(randomEssay.Id);
        
        // then
        actualEssay.Should().BeEquivalentTo(expectedEssay);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectEssayByIdAsync(randomEssay.Id),Times.Once);

        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls(); 
    }
}