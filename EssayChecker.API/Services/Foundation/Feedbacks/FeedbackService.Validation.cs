using System;
using System.Data;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;
using EssayChecker.API.Models.Foundation.Feedbacks;
using EssayChecker.API.Models.Foundation.Feedbacks.Exceptions;

namespace EssayChecker.API.Services.Foundation.Feedbacks;

public partial class FeedbackService
{
    private static void ValidateFeedbackOnAdd(Feedback feedback)
    {
        ValidateFeedbackIsNull(feedback);
        Validate((Rule: IsValid(feedback.Id), Parameter: nameof(Feedback.Id)),
            (Rule : IsValid(feedback.Comment), Parameter: nameof(Feedback.Comment)),
            (Rule : IsValid(feedback.Mark), Parameter: nameof(Feedback.Mark)));
    }

    private static void ValidateFeedbackIsNull(Feedback feedback)
    {
        if (feedback is null)
        {
            throw new FeedbackNullException();
        }
    }

    private static dynamic IsValid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };
    private static dynamic IsValid(string comment) => new
    {
        Condition = string.IsNullOrWhiteSpace(comment),
        Message = "Text is required"
    };
    private static dynamic IsValid(float mark) => new
    {
        Condition = mark < 0 || mark >= 9,
        Message = "Mark is required"
    };

    private static void ValidateFeedbackId(Guid id) =>
        Validate((Rule: IsValid(id), Parameter: nameof(Feedback.Id)));

    private static void ValidateStorageFeedback(Feedback feedback, Guid id)
    {
        if (feedback is null)
        {
            throw new NotFoundFeedbackException(id);
        }
    }
    
    private static void Validate(params (dynamic rule, string Parameter)[] validations)
    {
        var invalidFeedbackException = new InvalidFeedbackException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidFeedbackException.UpsertDataList(
                    key:parameter,
                    value:rule.Message);
            }
        }
        
        invalidFeedbackException.ThrowIfContainsErrors();
    }
}