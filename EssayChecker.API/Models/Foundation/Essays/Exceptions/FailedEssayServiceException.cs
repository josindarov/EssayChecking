using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.Essays.Exceptions;

public class FailedEssayServiceException : Xeption
{
    public FailedEssayServiceException(Exception innerException)
        : base("Failed essay service error occured, contact support", innerException)
    { }
}