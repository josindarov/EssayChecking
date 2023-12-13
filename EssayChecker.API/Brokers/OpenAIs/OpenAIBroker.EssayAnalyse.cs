using System.Threading.Tasks;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayChecker.API.Brokers.OpenAIs;

public partial class OpenAIBroker
{
    public async ValueTask<ChatCompletion> AnalyseEssayAsync(ChatCompletion chatCompletion)
    {
        return await _openAiClient.ChatCompletions.SendChatCompletionAsync(chatCompletion);
    }
}