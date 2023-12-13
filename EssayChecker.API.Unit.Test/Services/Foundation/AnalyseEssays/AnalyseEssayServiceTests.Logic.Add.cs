using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayServiceTests
{
    [Fact]
    public async Task ShouldAnalyseEssayAsync()
    {
        // given
        string randomText = GetRandomString();
        string inputText = randomText;
        string anotherRandomText = GetRandomString();
        string expectedText = anotherRandomText;
        
        ChatCompletion analyzedChatCompletion = CreateOutputChatCompletion(expectedText);

        openAiBrokerMock.Setup(broker => 
                broker.AnalyseEssayAsync(It.IsAny<ChatCompletion>()))
            .ReturnsAsync(analyzedChatCompletion);
		
        // when
        string actualText = await this.analyseEssayService.AnalyseEssayAsync(inputText);

        // then
        actualText.Should().BeEquivalentTo(expectedText);
        
        this.openAiBrokerMock.Verify(broker => 
            broker.AnalyseEssayAsync(It.IsAny<ChatCompletion>()),Times.Once);
        
        this.openAiBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}