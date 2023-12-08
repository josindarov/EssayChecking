using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class FeedbackNullException : Xeption
{
    public FeedbackNullException()
        : base("Feedback is null")
    { }
}