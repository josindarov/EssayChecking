using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Services.Foundation.Feedbacks;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Feedbacks;

public partial class FeedbackServiceTests
{
    private readonly Mock<IStorageBroker> storageBrokerMock;
    private readonly Mock<ILoggingBroker> loggingBrokerMock;
    private readonly IFeedbackService feedbackService;
    public FeedbackServiceTests()
    {
        this.storageBrokerMock = new Mock<IStorageBroker>();
        this.loggingBrokerMock = new Mock<ILoggingBroker>();
        this.feedbackService = new FeedbackService(
            storageBroker: storageBrokerMock.Object,
            loggingBroker: loggingBrokerMock.Object);
    }

    private static IQueryable<Feedback> CreateRandomEssays()
    {
        return CreateFeedbackFiller()
            .Create(count: GetRandomNumber())
            .AsQueryable();
    }
    
    private static string GetRandomString() =>
        new MnemonicString(wordCount: GetRandomNumber()).GetValue();
    
    private static int GetRandomNumber() =>
        new IntRange(min: 9, max: 99).GetValue();
    private static Feedback CreateRandomFeedback() =>
        CreateFeedbackFiller().Create();

    private static Filler<Feedback> CreateFeedbackFiller() =>
        new Filler<Feedback>();
    
    private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
        actualException => actualException.SameExceptionAs(expectedException);
    
    private static SqlException GetSqlException() =>
        (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
}