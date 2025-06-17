using System;
using System.Collections.Concurrent;
using CORE.Entities;
using CORE.Interfaces;

namespace Infrastructure.Data;

public class UnitOfWork(StoreContext ctx) : IUnitOfWork
{
    private readonly ConcurrentDictionary<string, object> _repositories = new();
    public async Task<bool> Complete()
    {
        return await ctx.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        ctx.Dispose();
    }

    public IGenericRepository<TEntity> genericRepository<TEntity>() where TEntity : BaseEntity
    {
        var type = typeof(TEntity).Name;

        return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
        {
            var repositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
            return Activator.CreateInstance(repositoryType, ctx) ??
            throw new InvalidOperationException($"Can't Create repository {t}");
        });
    }
}
