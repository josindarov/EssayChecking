using System.Threading.Tasks;

namespace EssayChecker.API.Services.Foundation.AnalyseEssays;

public interface IAnalyseEssayService
{
    ValueTask<string> AnalyseEssayAsync(string essay);
}