using Xeptions;

namespace EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;

public class NullAnalyseEssayException : Xeption
{
    public NullAnalyseEssayException()
        : base("This analyse is null")
    { }
}