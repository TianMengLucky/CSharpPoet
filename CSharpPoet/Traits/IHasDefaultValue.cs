namespace CSharpPoet.Traits;

public interface IHasDefaultValue
{
    public string? DefaultValue { get; set; }
}

internal static class IHasDefaultValueExtensions
{
    public static void WriteDefaultValueTo(this IHasDefaultValue self, CodeWriter writer)
    {
        if (self.DefaultValue is { } defaultValue)
        {
            writer.Write(" = ");
            writer.Write(defaultValue);
        }
    }
}
