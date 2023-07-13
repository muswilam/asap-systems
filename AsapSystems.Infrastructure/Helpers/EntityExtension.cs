using System.Linq.Expressions;

namespace AsapSystems.Infrastructure.Helpers
{
    public static class EntityExtension
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool isValid) where T : class =>
            isValid ? source.Where(predicate) : source;
    }
}