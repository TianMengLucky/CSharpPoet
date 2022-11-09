namespace CSharpPoet.Traits;

public interface IHasVisibility
{
    public Visibility? Visibility { get; set; }
}

internal static class IHasVisibilityExtensions
{
    public static void WriteVisibilityTo(this IHasVisibility self, CodeWriter writer)
    {
        if (self.Visibility is { } visibility)
        {
            writer.Write(visibility);
            writer.Write(' ');
        }
    }
}
