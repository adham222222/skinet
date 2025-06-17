using System;
using CORE.Entities;
using CORE.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Data;

public class SpecificationsEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

       
        if (spec.IsDistinct)
        {
            query = query.Distinct();
        }

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        query = spec.Includes.Aggregate(query,(current,include)=>current.Include(include));
        query = spec.IncludeStrings.Aggregate(query,(current,include)=>current.Include(include));
        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec, TResult>
    (IQueryable<T> query,
     ISpecification<T, TResult> spec)
    {
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        
        var SelectQuery = query as IQueryable<TResult>;

        if (spec.Select != null)
        {
            SelectQuery = query.Select(spec.Select);
        }

        if (spec.IsDistinct)
        {
            SelectQuery = SelectQuery?.Distinct();
        }

        if (spec.IsPagingEnabled)
        {
            SelectQuery = SelectQuery?.Skip(spec.Skip).Take(spec.Take);
        }
        
        return SelectQuery ?? query.Cast<TResult>();
    }

}
