using EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;

namespace EssayChecker.API.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayService
{
    private static void ValidateAnalysedEssayOnAdd(string essay)
    {
        ValidateAnalysedEssayIsNotNull(essay);
    }
    
    private static void ValidateAnalysedEssayIsNotNull(string essay)
    {
        if (essay is null)
        {
            throw new NullAnalyseEssayException();
        }
    }
}