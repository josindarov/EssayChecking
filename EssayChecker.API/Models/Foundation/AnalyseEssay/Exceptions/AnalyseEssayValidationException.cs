using System;
using Xeptions;

namespace EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;

public class AnalyseEssayValidationException : Xeption
{
    public AnalyseEssayValidationException(Exception innerException)
        : base("Analyse Essay validation exception occured, fix and try again.",
            innerException)
    { }
}