using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Essays;

namespace EssayChecker.API.Services.Foundation.Essays;

public class EssayService : IEssayService
{
    private readonly ILoggingBroker loggingBroker;
    private readonly IStorageBroker storageBroker;
    public EssayService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }
    public async ValueTask<Essay> InsertEssayAsync(Essay essay)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Essay> SelectAllEssays()
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Essay> SelectEssayById(Guid id)
    {
        throw new NotImplementedException();
    }
}