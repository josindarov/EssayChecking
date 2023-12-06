using System;
using System.Data;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;

namespace EssayChecker.API.Services.Foundation.Essays;

public partial class EssayService
{
    private static void ValidateEssayOnAdd(Essay essay)
    {
        ValidateEssayNotNull(essay);
        Validate((Rule: IsInvalid(essay.Id), Parameter: nameof(Essay.Id)),
            (Rule: IsInvalid(essay.EssayContent), Parameter: nameof(Essay.EssayContent)),
            (Rule: IsInvalid(essay.EssayType), Parameter: nameof(Essay.EssayType)));
    }
    private static void ValidateEssayNotNull(Essay essay)
    {
        if (essay is null)
        {
            throw new EssayNullException();
        }
    }
    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };
    
    private static dynamic IsInvalid(string name) => new
    {
        Condition = string.IsNullOrWhiteSpace(name),
        Message = "Text is required"
    };
    
    private void ValidateEssayId(Guid id) =>
        Validate((Rule: IsInvalid(id), Parameter: nameof(Essay.Id)));
    
    private static void ValidateStorageEssay(Essay essay, Guid essayId)
    {
        if (essay is null)
        {
            throw new NotFoundEssayException(essayId);
        }
    }
    
    private static void Validate(params (dynamic rule, string Parameter)[] validations)
    {
        var invalidEssayException = new InvalidEssayException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidEssayException.UpsertDataList(
                    key:parameter,
                    value:rule.Message);
            }
        }
        
        invalidEssayException.ThrowIfContainsErrors();
    }
}