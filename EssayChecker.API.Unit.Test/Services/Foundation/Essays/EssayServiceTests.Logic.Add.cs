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
    public async Task ShouldEssayAddAsync()
    {
        // given
        Essay randomEssay = CreateRandomEssay();
        Essay inputEssay = randomEssay;
        Essay persistedEssay = inputEssay;
        Essay expectedEssay = persistedEssay.DeepClone();

        this.storageBrokerMock.Setup(broker =>
            broker.InsertEssayAsync(inputEssay)).ReturnsAsync(persistedEssay);
        
        // when
        Essay actualEssay = await this.essayService
                .InsertEssayAsync(inputEssay);
        
        // then 
        actualEssay.Should().BeEquivalentTo(expectedEssay);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertEssayAsync(inputEssay),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}