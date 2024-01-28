
namespace Infrastructure.ExtensionMethods;

public static class IQueryableExtensionMethods
{
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int page, int length)
        => length > 0 ? query.Skip(page).Take(length) : query;
}
