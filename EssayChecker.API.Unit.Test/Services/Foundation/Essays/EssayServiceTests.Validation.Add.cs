using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfEssayIsNullAndLogItAsync()
    {
        // given
        Essay nullEssay = null;
        var essayNullException = new EssayNullException();
        
        var expectedEssayValidationException = 
            new EssayValidationException(essayNullException);
        
        // when
        ValueTask<Essay> addEssayTask = this.essayService.InsertEssayAsync(nullEssay);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(addEssayTask.AsTask);
        
        // then
        actualEssayValidationException.Should()
            .BeEquivalentTo(expectedEssayValidationException);
        
        this.loggingBrokerMock.Verify(broker => 
            broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ShouldThrowValidationExceptionOnAddIfEssayIsInvalidAndLogItAsync(
        string invalidText)
    {
        // given
        Essay invalidEssay = new Essay()
        {
            EssayContent = invalidText,
            EssayType = invalidText
        };
        InvalidEssayException invalidEssayException = new InvalidEssayException();
        
        invalidEssayException.AddData(
            key:nameof(Essay.Id),
            values:"Id is required");
        
        invalidEssayException.AddData(
            key:nameof(Essay.EssayContent),
            values:"Text is required");
        
        invalidEssayException.AddData(
            key:nameof(Essay.EssayType),
            values:"Text is required");
        
        var expectedEssayValidationException = 
            new EssayValidationException(invalidEssayException);
        
        // when
        ValueTask<Essay> addEssayTask = this.essayService.InsertEssayAsync(invalidEssay);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(addEssayTask.AsTask);

        // then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
            broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Once);
        
        this.storageBrokerMock.Verify(broker => 
            broker.InsertEssayAsync(invalidEssay), Times.Never);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
    
}