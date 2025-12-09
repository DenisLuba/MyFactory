using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public abstract class Specification<T> : ISpecification<T>
{
    protected Specification(Expression<Func<T, bool>>? criteria = null)
    {
        Criteria = criteria;
    }

    public Expression<Func<T, bool>>? Criteria { get; protected set; }
    public List<Expression<Func<T, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; protected set; }
    public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByDescending { get; protected set; }
    public int? Take { get; protected set; }
    public int? Skip { get; protected set; }
    public bool AsNoTracking { get; protected set; }
    public bool IncludeSoftDeleted { get; protected set; }

    IReadOnlyCollection<Expression<Func<T, object>>> ISpecification<T>.Includes => Includes;
    IReadOnlyCollection<string> ISpecification<T>.IncludeStrings => IncludeStrings;

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
        => Includes.Add(includeExpression);

    protected void AddInclude(string includeString)
        => IncludeStrings.Add(includeString);

    protected void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
        => OrderBy = orderByExpression;

    protected void ApplyOrderByDescending(Func<IQueryable<T>, IOrderedQueryable<T>> orderByDescExpression)
        => OrderByDescending = orderByDescExpression;

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    protected void AsNoTrackingQuery()
        => AsNoTracking = true;

    protected void IncludeDeleted()
        => IncludeSoftDeleted = true;
}
