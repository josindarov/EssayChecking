using System;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.AnalyseEssay.Exceptions;

namespace EssayChecker.API.Services.Foundation.AnalyseEssays;

public partial class AnalyseEssayService
{
    private delegate ValueTask<string> ReturnAnalysedEssayAsync();

    private async ValueTask<string> TryCatch(ReturnAnalysedEssayAsync returnAnalysedEssayAsync)
    {
        try
        {
            return await returnAnalysedEssayAsync();
        }
        catch (NullAnalyseEssayException nullAnalyseEssayException)
        {
            throw CreateAndLogValidationException(nullAnalyseEssayException);
        }
    }

    private AnalyseEssayValidationException CreateAndLogValidationException(Exception exception)
    {
        var analyseEssayValidationException =
            new AnalyseEssayValidationException(exception);
        
        this.loggingBroker.LogError(analyseEssayValidationException);
        return analyseEssayValidationException;
    }
}