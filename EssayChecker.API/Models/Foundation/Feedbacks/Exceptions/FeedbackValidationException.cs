using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class FeedbackValidationException : Xeption
{
    public FeedbackValidationException(Exception innerException)
        : base("Validation error occured, fix it and try again",
            innerException)
    { }
}