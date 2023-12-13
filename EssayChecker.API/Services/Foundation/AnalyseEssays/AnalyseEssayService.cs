using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.OpenAIs;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayChecker.API.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayService : IAnalyseEssayService
{
    private readonly IOpenAIBroker openAiBroker;
    private readonly ILoggingBroker loggingBroker;

    public AnalyseEssayService(IOpenAIBroker openAiBroker,
        ILoggingBroker loggingBroker)
    {
        this.openAiBroker = openAiBroker;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<string> AnalyseEssayAsync(string essay) =>
        TryCatch(async () =>
    {
        ValidateAnalysedEssayOnAdd(essay);
        ChatCompletion request = CreateRequest(essay);

        ChatCompletion response = await this.openAiBroker
            .AnalyseEssayAsync(request);

        return response.Response.Choices.FirstOrDefault().Message.Content;
    });

    private static ChatCompletion CreateRequest(string essay)
    {
        var request = new ChatCompletion
        {
            Request = new ChatCompletionRequest
            {
                Model = "gpt-3.5-turbo",
                Messages = new ChatCompletionMessage[]
                {
                    new ChatCompletionMessage
                    {
                        Role = "System",
                        Content =
                            "You are IELTS Writing examiner. Give detailed IELTS score based on marking criteria of IELTS."
                    },
                    new ChatCompletionMessage
                    {
                        Role = "User",
                        Content = essay
                    }
                }
            }
        };
		
        return request;

    }
}