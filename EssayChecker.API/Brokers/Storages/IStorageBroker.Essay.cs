using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;

namespace EssayChecker.API.Brokers.Storages;

public partial interface IStorageBroker
{
    ValueTask<Essay> InsertEssayAsync(Essay essay);
    ValueTask<Essay> SelectEssayByIdAsync(Guid id);
    IQueryable<Essay> SelectAllEssays();
    
}