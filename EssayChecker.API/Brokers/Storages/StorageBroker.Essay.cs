using System;
using System.Linq;
using System.Threading.Tasks;
using EssayChecker.API.Models.Foundation.Essays;
using Microsoft.EntityFrameworkCore;

namespace EssayChecker.API.Brokers.Storages;

public partial class StorageBroker : IStorageBroker
{
    public DbSet<Essay> Essays { get; set; }

    public async ValueTask<Essay> InsertEssayAsync(Essay essay) =>
        await InsertAsync(essay);

    public async ValueTask<Essay> SelectEssayByIdAsync(Guid id) =>
        await SelectAsync<Essay>(id);

    public IQueryable<Essay> SelectAllEssays() =>
        SelectAll<Essay>();
}