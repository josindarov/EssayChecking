using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using System.Threading.Tasks;

namespace EssayChecker.API.Brokers.OpenAI
{
    public partial interface IOpenAIBroker
    {
        ValueTask<ChatCompletion> AnalyseEssayAsync(ChatCompletion chatCompletion);
    }
}
