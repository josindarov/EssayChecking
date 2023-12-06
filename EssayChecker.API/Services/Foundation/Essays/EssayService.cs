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
    public async ValueTask<Essay> InsertEssayAsync(Essay essay)
    {
        try
        {
            if (essay is null)
            {
                throw new EssayNullException();
            }
            return await this.storageBroker.InsertEssayAsync(essay);
        }
        catch (EssayNullException essayNullException)
        {
            EssayValidationException essayValidationException = 
                new EssayValidationException(essayNullException);

            throw essayValidationException;
        }
        
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