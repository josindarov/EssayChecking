using Microsoft.Extensions.Configuration;
using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using System.Threading.Tasks;

namespace EssayChecker.API.Brokers.OpenAI;

public partial class OpenAIBroker : IOpenAIBroker
{
    private readonly IOpenAIClient openAIClient;
    private readonly IConfiguration configuration;

    public OpenAIBroker(IConfiguration configuration)
    {
        this.configuration = configuration;
        openAIClient = ConfigureOpenAIClient();
    }

    private IOpenAIClient ConfigureOpenAIClient()
    {
        var apiKey = configuration
            .GetValue<string>(key: "OpenAIKey");

        var openAIConfiguration = new OpenAIConfigurations()
        {
            ApiKey = apiKey
        };

        return new OpenAIClient(openAIConfiguration);
    }
}