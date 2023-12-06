using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Users;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    [Fact]
    public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAndLogItAsync()
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
            Times.Once);
        
        this.loggingBrokerMock.VerifyNoOtherCalls();
        this.storageBrokerMock.VerifyNoOtherCalls();
    }
}