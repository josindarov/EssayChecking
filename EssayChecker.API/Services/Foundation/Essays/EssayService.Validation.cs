using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;

namespace EssayChecker.API.Services.Foundation.Essays;

public partial class EssayService
{
    private static void ValidateEssayNotNull(Essay essay)
    {
        if (essay is null)
        {
            throw new EssayNullException();
        }
    }
}