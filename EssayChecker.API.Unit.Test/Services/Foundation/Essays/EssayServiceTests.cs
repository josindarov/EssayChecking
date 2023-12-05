using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Services.Foundation.Essays;
using Moq;
using Tynamix.ObjectFiller;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Essays;

public partial class EssayServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IEssayService essayService;
    
    public EssayServiceTests()
    {
        storageBrokerMock = new Mock<IStorageBroker>();
        loggingBrokerMock = new Mock<ILoggingBroker>();
        essayService = new EssayService(
            storageBroker:storageBrokerMock.Object,
            loggingBroker: loggingBrokerMock.Object);
    }

    private static Essay CreateRandomEssay() =>
        CreateEssayFiller().Create();

    private static Filler<Essay> CreateEssayFiller() =>
        new Filler<Essay>();

}