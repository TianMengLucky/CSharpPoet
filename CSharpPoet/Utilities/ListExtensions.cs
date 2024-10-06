namespace CSharpPoet;

/// <summary>
///     Utilities for <see cref="IList{T}" />
/// </summary>
public static class ListExtensions
{
    /// <summary>
    ///     Adds the elements of the specified collection to the end of the <see cref="IList{T}" />.
    /// </summary>
    /// <param name="list">The list for the items to be added to.</param>
    /// <param name="items">
    ///     The collection whose elements should be added to the end of the <see cref="IList{T}" />. The
    ///     collection itself cannot be null, but it can contain elements that are null, if type T is a reference type
    /// </param>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="list"></paramref> or <paramref name="items"></paramref> is
    ///     null.
    /// </exception>
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
        if (list == null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        if (list is List<T> asList)
        {
            asList.AddRange(items);
        }
        else
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }
    }
}
