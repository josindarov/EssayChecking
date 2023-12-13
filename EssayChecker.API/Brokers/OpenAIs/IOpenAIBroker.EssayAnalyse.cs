using System.Threading.Tasks;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayChecker.API.Brokers.OpenAIs;

public partial interface IOpenAIBroker
{
    ValueTask<ChatCompletion> AnalyseEssayAsync(ChatCompletion chatCompletion);
}