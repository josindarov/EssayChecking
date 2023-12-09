using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class FailedFeedbackStorageException : Xeption
{
    public FailedFeedbackStorageException(Exception innerException)
        : base("Failed feedback storage error occured, contact support.", innerException)
    { }
}