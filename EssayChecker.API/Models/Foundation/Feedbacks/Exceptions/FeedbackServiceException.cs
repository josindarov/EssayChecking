using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class FeedbackServiceException : Xeption
{
    public FeedbackServiceException(Exception innerException)
        : base("Feedback service exception occured, contact support", innerException)
    { }
}