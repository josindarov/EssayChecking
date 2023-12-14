using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;

public class AnalyseEssayServiceException : Xeption
{
    public AnalyseEssayServiceException(Exception innerException)
        : base("Service error occured, contact support.",innerException)
    { }
}