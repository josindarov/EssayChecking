using System;
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
    public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
    {
        // given
        Guid invalidEssayId = Guid.Empty;
        var invalidEssayException = new InvalidEssayException();
        
        invalidEssayException.AddData(
            key:nameof(Essay.Id),
            values:"Id is required");

        var expectedEssayValidationException = 
            new EssayValidationException(invalidEssayException);
        
        // when
        ValueTask<Essay> retrieveEssayByIdTask = 
            this.essayService.RetrieveEssayById(invalidEssayId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(retrieveEssayByIdTask.AsTask);
        
        // then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
        
        this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedEssayValidationException))),
            Times.Once);

        this.storageBrokerMock.Verify(broker =>
                broker.SelectEssayByIdAsync(It.IsAny<Guid>()),
            Times.Never);

        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();

    }
    
    [Fact]
    public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfEssayIsNotFoundAndLogItAsync()
    {
        // given
        Guid someEssayId = Guid.NewGuid();
        Essay noEssay = null;
        var notFoundEssayException = new NotFoundEssayException(someEssayId);

        var expectedEssayValidationException = 
            new EssayValidationException(notFoundEssayException);

        this.storageBrokerMock.Setup(broker => 
            broker.SelectEssayByIdAsync(It.IsAny<Guid>())).ReturnsAsync(noEssay);
        
        // when
        ValueTask<Essay> retrieveEssayByIdTask = this.essayService.RetrieveEssayById(someEssayId);

        EssayValidationException actualEssayValidationException =
            await Assert.ThrowsAsync<EssayValidationException>(retrieveEssayByIdTask.AsTask);

        // then
        actualEssayValidationException.Should().BeEquivalentTo(expectedEssayValidationException);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectEssayByIdAsync(It.IsAny<Guid>()),Times.Once);
        
        this.loggingBrokerMock.Verify(broker => 
                broker.LogError(It.Is(SameExceptionAs(expectedEssayValidationException))),
            Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}