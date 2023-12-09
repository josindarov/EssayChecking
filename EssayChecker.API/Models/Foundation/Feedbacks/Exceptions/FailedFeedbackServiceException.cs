using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class FailedFeedbackServiceException : Xeption
{
    public FailedFeedbackServiceException(Exception innerException)
        : base("Failed feedback service occured, contact support.", innerException)
    { }
}