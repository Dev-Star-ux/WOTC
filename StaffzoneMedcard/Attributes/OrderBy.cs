using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
public static class QueryableExtensions
    {
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return source;

        // Split by comma for multiple orderings
        var orderings = orderBy.Split(',');

        IOrderedQueryable<T> orderedQuery = null;

        foreach (var ordering in orderings)
        {
            var trimmed = ordering.Trim();
            if (string.IsNullOrEmpty(trimmed)) continue;

            // Split property and direction
            var parts = trimmed.Split(' ');
            var propertyName = parts[0];
            var descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            string methodName;
            if (orderedQuery == null)
                methodName = descending ? "OrderByDescending" : "OrderBy";
            else
                methodName = descending ? "ThenByDescending" : "ThenBy";

            var result = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName
                            && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { orderedQuery ?? source, lambda });

            orderedQuery = (IOrderedQueryable<T>)result;
        }

        return orderedQuery ?? source;
    }
}
