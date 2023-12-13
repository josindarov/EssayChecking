using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayChecker.API.Brokers.OpenAIs;

public partial class OpenAIBroker : IOpenAIBroker
{
    private readonly IOpenAIClient _openAiClient;
    private readonly IConfiguration _configuration;

    public OpenAIBroker(IConfiguration configuration)
    {
        _configuration = configuration;
        _openAiClient = ConfigureOpenAiClient();
    }

    private IOpenAIClient ConfigureOpenAiClient()
    {
        var apiKey = _configuration.GetValue<string>(key:"OpenAIKey");
        var openAiConfiguration = new OpenAIConfigurations()
        {
            ApiKey = apiKey
        }; 
        
        return new OpenAIClient(openAiConfiguration);
    }
}