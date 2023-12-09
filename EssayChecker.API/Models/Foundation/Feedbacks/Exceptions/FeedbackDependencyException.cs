using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class FeedbackDependencyException : Xeption
{
    public FeedbackDependencyException(Exception innerException)
        : base("Feedback dependency error occured, contact support.", innerException)
    { }
}