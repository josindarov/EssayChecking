using System;
using System.Linq;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.OpenAIs;
using EssayChecker.API.Services.Foundation.AnalyseEssays;
using Moq;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;
using Tynamix.ObjectFiller;

namespace EssayChecker.API.Unit.Test.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayServiceTests
{
    private readonly Mock<IOpenAIBroker> openAiBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IAnalyseEssayService analyseEssayService;

    public AnalyseEssayServiceTests()
    {
        this.openAiBrokerMock = new Mock<IOpenAIBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();
        this.analyseEssayService = new AnalyseEssayService(
            openAiBroker: openAiBrokerMock.Object,
            loggingBroker: loggingBrokerMock.Object);
    }

    private static int GetRandomNumber() =>
        new IntRange(min: 0, max: 99).GetValue();

    private static string GetRandomString() =>
        new MnemonicString(wordCount: GetRandomNumber()).GetValue();

    private static DateTimeOffset GetRandomDateTimeOffSet() =>
        new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();
    private static ChatCompletion CreateOutputChatCompletion(string essay) =>
        CreateOutputChatCompletionFiller(essay).Create();

    private static Filler<ChatCompletion> CreateOutputChatCompletionFiller(string essay)
    {
        var filler = new Filler<ChatCompletion>();

        filler.Setup().OnProperty(chatCompletion => chatCompletion.Response.Choices.FirstOrDefault().Message.Content)
            .Use(essay);

        filler.Setup().OnType<DateTimeOffset>().Use(GetRandomDateTimeOffSet);

        return filler;
    }
}