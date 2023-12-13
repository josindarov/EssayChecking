using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionIfAnalyseIsNullAndLogItAsync()
    {
        // given
        string nullAnalyseEssay = null;
        var nullAnalyseEssayException = new NullAnalyseEssayException();
        
        var expectedAnalyseEssayValidationException =
            new AnalyseEssayValidationException(nullAnalyseEssayException);
        
        // when 
        ValueTask<string> analyseEssayTask = this.analyseEssayService.AnalyseEssayAsync(nullAnalyseEssay);

        AnalyseEssayValidationException actualAnalyseEssayValidationException =
            await Assert.ThrowsAsync<AnalyseEssayValidationException>(analyseEssayTask.AsTask);
        
        // then
        actualAnalyseEssayValidationException.Should()
            .BeEquivalentTo(expectedAnalyseEssayValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedAnalyseEssayValidationException))),
            Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.openAiBrokerMock.VerifyNoOtherCalls();
    }
}