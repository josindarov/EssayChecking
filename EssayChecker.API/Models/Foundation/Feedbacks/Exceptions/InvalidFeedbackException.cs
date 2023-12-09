using Xeptions;

namespace EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

public class InvalidFeedbackException : Xeption
{
    public InvalidFeedbackException()
        : base("Feedback is invalid.")
    { }
}