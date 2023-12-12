using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using System.Threading.Tasks;

namespace EssayChecker.API.Brokers.OpenAI
{
    public partial class OpenAIBroker
    {
        public async ValueTask<ChatCompletion> AnalyseEssayAsync(ChatCompletion chatCompletion)
        {
            return await openAIClient.ChatCompletions.SendChatCompletionAsync(chatCompletion);
        }
    }
}
