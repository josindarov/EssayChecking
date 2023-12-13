using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.OpenAIs;

namespace EssayChecker.API.Services.Foundation.AnalyseEssays;

public class AnalyseEssayService : IAnalyseEssayService
{
    private readonly IOpenAIBroker openAiBroker;
    private readonly ILoggingBroker loggingBroker;

    public AnalyseEssayService(IOpenAIBroker openAiBroker,
        ILoggingBroker loggingBroker)
    {
        this.openAiBroker = openAiBroker;
        this.loggingBroker = loggingBroker;
    }
    
    public async ValueTask<string> AnalyseEssayAsync(string essay)
    {
        throw new System.NotImplementedException();
    }
}