using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace EssayChecker.API.Services.Foundation.Feedbacks;


public partial class FeedbackService
{
    private delegate ValueTask<Feedback> ReturningFeedbackFunction();

    private async ValueTask<Feedback> TryCatch(ReturningFeedbackFunction returningFeedbackFunction)
    {
        try
        {
            return await returningFeedbackFunction();
        }
        catch (FeedbackNullException feedbackNullException)
        {
            throw CreateAndLogValidationException(feedbackNullException);
        }
        catch (InvalidFeedbackException invalidFeedbackException)
        {
            throw CreateAndLogValidationException(invalidFeedbackException);
        }
        catch (SqlException sqlException)
        {
            var failedFeedbackStorageException =
                new FailedFeedbackStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedFeedbackStorageException);
        }
        catch (Exception exception)
        {
            var failedFeedbackServiceException =
                new FailedFeedbackServiceException(exception);

            throw CreateAndLogServiceException(failedFeedbackServiceException);
        }
    }

    private Exception CreateAndLogServiceException(Exception exception)
    {
        var feedbackServiceException = 
            new FeedbackServiceException(exception);

        this.loggingBroker.LogError(feedbackServiceException);
        return feedbackServiceException;
    }

    private FeedbackDependencyException CreateAndLogCriticalDependencyException(Exception exception)
    {
        var feedbackDependencyException = new FeedbackDependencyException(exception);
        this.loggingBroker.LogCritical(feedbackDependencyException);
        return feedbackDependencyException;
    }

    private FeedbackValidationException CreateAndLogValidationException(Xeption exception)
    {
        var feedbackValidationException = new FeedbackValidationException(exception);
        loggingBroker.LogError(feedbackValidationException);
        return feedbackValidationException;
    }
}