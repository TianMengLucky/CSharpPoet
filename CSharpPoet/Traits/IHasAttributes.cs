namespace CSharpPoet.Traits;

/// <summary>
/// Represents a member that has attributes.
/// </summary>
public interface IHasAttributes
{
    /// <summary>
    /// Gets or sets a list of attributes assigned to this member.
    /// </summary>
    public IList<CSharpAttribute> Attributes { get; set; }
}

internal static class IHasAttributesExtensions
{
    public static void WriteAttributesTo(this IHasAttributes self, CodeWriter writer)
    {
        foreach (var attribute in self.Attributes)
        {
            attribute.WriteTo(writer);
            writer.WriteLine();
        }
    }
}
