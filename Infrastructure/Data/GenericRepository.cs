using System;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class GenericRepository<T>(StoreContext ctx) : IGenericRepository<T> where T : BaseEntity
{
    public void Add(T entity)
    {
        ctx.Set<T>().Add(entity);
    }
    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        var query = ctx.Set<T>().AsQueryable();

        query = spec.ApplyCriteria(query);

        return await query.CountAsync();
    }
    public void Delete(T entity)
    {
        ctx.Set<T>().Remove(entity);
    }

    public bool Exists(int id)
    {
        return ctx.Set<T>().Any(x => x.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await ctx.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await ctx.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetItemWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<TResult?> GetItemWithSpec<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    
    public void Update(T entity)
    {
        ctx.Set<T>().Attach(entity);
        ctx.Set<T>().Entry(entity).State = EntityState.Modified;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationsEvaluator<T>.GetQuery(ctx.Set<T>().AsQueryable(), spec);
    }
    
    private  IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T,TResult> spec)
    {
        return SpecificationsEvaluator<T>.GetQuery<T,TResult>(ctx.Set<T>().AsQueryable(), spec);
    }
}
