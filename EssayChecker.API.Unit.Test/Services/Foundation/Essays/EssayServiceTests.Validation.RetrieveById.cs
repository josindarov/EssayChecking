using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;
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
}