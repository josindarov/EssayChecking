using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Brokers.Loggings;
using EssayChecker.API.Brokers.Storages;
using EssayChecker.API.Models.Foundation.Essays;
using EssayChecker.API.Models.Foundation.Essays.Exceptions;

namespace EssayChecker.API.Services.Foundation.Essays;

public partial class EssayService : IEssayService
{
    private readonly ILoggingBroker loggingBroker;
    private readonly IStorageBroker storageBroker;
    public EssayService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<Essay> InsertEssayAsync(Essay essay) =>
        TryCatch(async () =>
        {
            ValidateEssayOnAdd(essay);
            return await this.storageBroker.InsertEssayAsync(essay);
        });

    public IQueryable<Essay> RetrieveAllEssays()
    {
        return this.storageBroker.SelectAllEssays();
    }

    public ValueTask<Essay> RetrieveEssayById(Guid id) =>
        TryCatch(async () =>
        {
            ValidateEssayId(id);
            Essay essay = await this.storageBroker.SelectEssayByIdAsync(id);
            ValidateStorageEssay(essay, id);
            return essay;
        });
}