using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;
using FluentAssertions;
using Moq;
using Xunit;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    [Fact]
    public async Task ShouldRetrieveAllFeedbacks()
    {
        // given 
        IQueryable<Feedback> randomFeedbacks = CreateRandomEssays();
        IQueryable<Feedback> storagedFeedbacks = randomFeedbacks;
        IQueryable<Feedback> expectedFeedbacks = storagedFeedbacks;

        this.storageBrokerMock.Setup(broker =>
            broker.SelectAllFeedbacks()).Returns(storagedFeedbacks);
        
        // when 
        IQueryable<Feedback> actualFeedbacks = this.feedbackService.RetrieveAllFeedbacks();
        
        // then 
        actualFeedbacks.Should().BeEquivalentTo(expectedFeedbacks);
        
        this.storageBrokerMock.Verify(broker => 
            broker.SelectAllFeedbacks(),Times.Once);
        
        this.storageBrokerMock.VerifyNoOtherCalls();
        this.loggingBrokerMock.VerifyNoOtherCalls();
    }
}