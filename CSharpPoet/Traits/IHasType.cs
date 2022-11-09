namespace CSharpPoet.Traits;

public interface IHasType
{
    public string Type { get; set; }
}

internal static class IHasTypeExtensions
{
    public static void WriteTypeTo(this IHasType self, CodeWriter writer)
    {
        writer.Write(self.Type);
        writer.Write(' ');
    }
}
