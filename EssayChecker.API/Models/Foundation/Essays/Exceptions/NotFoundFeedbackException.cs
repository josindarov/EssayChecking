using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class NotFoundFeedbackException : Xeption
{
    public NotFoundFeedbackException(Guid id)
        : base($"Feedback is not found in {id} id.")
    { }
}