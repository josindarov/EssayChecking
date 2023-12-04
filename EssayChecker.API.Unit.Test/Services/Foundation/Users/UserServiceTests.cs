using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using EssayChecker.API.Brokers.DateTimes;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Services.Foundation.Users;
using Microsoft.Data.SqlClient;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;
using Xunit;
using Xunit.Sdk;

namespace EssayChecker.API.Unit.Test.Services.Foundation.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;

        public UserServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            
            this.userService = new UserService(
                storageBroker: storageBrokerMock.Object,
                loggingBroker:loggingBrokerMock.Object,
                dateTimeBroker: dateTimeBrokerMock.Object);
        }
        public static TheoryData<int> InvalidMinutes()
        {
            int minutesInFuture = GetRandomNumber();
            int minutesInPast = GetRandomNegativeNumber();

            return new TheoryData<int>
            {
                minutesInFuture,
                minutesInPast
            };
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 9, max: 99).GetValue();

        private static int GetRandomNegativeNumber() =>
            -1 * new IntRange(min: 9, max: 99).GetValue();
        
        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
			actualException => actualException.SameExceptionAs(expectedException);

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));
        
        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();
             
        

        private static User CreateRandomUser() =>
            CreateUserFiller().Create();

        private static Filler<User> CreateUserFiller() =>
            new Filler<User>();


    }
}
