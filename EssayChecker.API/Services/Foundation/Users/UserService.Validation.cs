using System;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using EssayChecker.API.Models.Foundation.Users;
using EssayChecker.API.Models.Foundation.Users.Exceptions;

namespace EssayChecker.API.Services.Foundation.Users;

public partial class UserService
{
    private static void ValidateUserOnAdd(User user)
    {
        ValidateUserNotNull(user);
        Validate((Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
            (Rule: IsInvalid(user.Name), Parameter: nameof(User.Name)),
            (Rule: IsInvalid(user.TelephoneNumber), Parameter: nameof(User.TelephoneNumber)));
    }
    
    private static void ValidateUserOnModify(User user)
    {
        ValidateUserNotNull(user);
        Validate((Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
            (Rule: IsInvalid(user.Name), Parameter: nameof(User.Name)),
            (Rule: IsInvalid(user.TelephoneNumber), Parameter: nameof(User.TelephoneNumber)));
    }
    private static void ValidateUserNotNull(User user)
    {
        if (user is null)
        {
            throw new UserNullException();
        }
    }

    private static void ValidateStorageUser(User user, Guid userId)
    {
        if (user is null)
        {
            throw new NotFoundUserException(userId);
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

    private static void Validate(params (dynamic rule, string Parameter)[] validations)
    {
        var invalidUserException = new InvalidUserException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidUserException.UpsertDataList(
                    key:parameter,
                    value:rule.Message);
            }
        }
        
        invalidUserException.ThrowIfContainsErrors();
    }
    
    
}