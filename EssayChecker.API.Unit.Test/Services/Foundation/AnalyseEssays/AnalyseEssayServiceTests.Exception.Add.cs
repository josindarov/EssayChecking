using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayServiceTests
{
    [Fact]
    public async Task ShouldThrowServiceExceptionOnAnalyseEssayIfServiceExceptionOccuredAndLogItAsync()
    {
        // given
        string randomText = GetRandomString();
        Exception serviceException = new Exception();
        
        var expectedAnalyseEssayServiceException =
            new AnalyseEssayServiceException(serviceException);

        this.openAiBrokerMock.Setup(broker =>
            broker.AnalyseEssayAsync(It.IsAny<ChatCompletion>()))
            .Throws(serviceException);
        
        // when
        ValueTask<string> analyseEssayTask = this.analyseEssayService.AnalyseEssayAsync(randomText);

        AnalyseEssayServiceException actualAnalyseEssayServiceException =
            await Assert.ThrowsAsync<AnalyseEssayServiceException>(analyseEssayTask.AsTask);
        
        // then
        actualAnalyseEssayServiceException.Should().BeEquivalentTo(expectedAnalyseEssayServiceException);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedAnalyseEssayServiceException))),
            Times.Once);
        
        this.openAiBrokerMock.Verify(broker => 
                broker.AnalyseEssayAsync(It.IsAny<ChatCompletion>()),
            Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.openAiBrokerMock.VerifyNoOtherCalls();
    }
}