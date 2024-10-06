namespace CSharpPoet.Traits;

public interface IHasDefaultValue
{
    public Value? DefaultValue { get; set; }
}

internal static class IHasDefaultValueExtensions
{
    public static void WriteDefaultValueTo(this IHasDefaultValue self, CodeWriter writer)
    {
        if (self.DefaultValue is not { } defaultValue)
        {
            return;
        }

        defaultValue.Write(writer);
    }
}
