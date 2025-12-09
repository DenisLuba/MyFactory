using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MyFactory.Infrastructure.Repositories.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; }
    IReadOnlyCollection<Expression<Func<T, object>>> Includes { get; }
    IReadOnlyCollection<string> IncludeStrings { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; }
    Func<IQueryable<T>, IOrderedQueryable<T>>? OrderByDescending { get; }
    int? Take { get; }
    int? Skip { get; }
    bool AsNoTracking { get; }
    bool IncludeSoftDeleted { get; }
}
