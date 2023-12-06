using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;

namespace EssayChecker.API.Services.Foundation.Essays;

public interface IEssayService
{
    ValueTask<Essay> InsertEssayAsync(Essay essay);
    IQueryable<Essay> RetrieveAllEssays();
    ValueTask<Essay> RetrieveEssayById(Guid id);
}