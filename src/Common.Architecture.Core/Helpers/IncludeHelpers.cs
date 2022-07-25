using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Common.Architecture.Core.Helpers
{
    public class IncludeHelpers<TEntity>
    {
        public Expression<Func<TEntity, object>> Include { get; set; }

        public List<Expression<Func<IIncludableQueryable<TEntity, TEntity>>>> ThenIncludes { get; set; }

        //public static IQueryable<TEntity> Include<TEntity>(
        //    [NotNullAttribute] this IQueryable<TEntity> source,
        //    [NotNullAttribute][NotParameterized] string navigationPropertyPath)
        //    where TEntity : class;
        //public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
        //    [NotNullAttribute] this IQueryable<TEntity> source, 
        //    [NotNullAttribute] Expression<Func<TEntity, TProperty>> navigationPropertyPath) 
        //    where TEntity : class;

        //public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNullAttribute] this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, 
        //    [NotNullAttribute] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) 
        //    where TEntity : class;

        //public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
        //    [NotNullAttribute] this IIncludableQueryable<TEntity, TPreviousProperty> source, 
        //    [NotNullAttribute] Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) 
        //    where TEntity : class;

    }
}