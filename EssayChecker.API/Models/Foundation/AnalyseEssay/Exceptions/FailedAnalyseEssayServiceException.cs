using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;

public class FailedAnalyseEssayServiceException : Xeption
{
    public FailedAnalyseEssayServiceException(Exception innerException)
        : base("Failed analyse essay service error occured, contact support.", innerException)
    { }
}