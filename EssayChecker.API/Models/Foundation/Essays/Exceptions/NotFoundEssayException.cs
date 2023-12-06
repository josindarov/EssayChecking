using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class NotFoundEssayException : Xeption
{
    public NotFoundEssayException(Guid essayId)
        : base($"Essay is not found in {essayId} id")
    { }
}