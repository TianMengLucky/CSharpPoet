namespace CSharpPoet.Traits;

public interface IHasName
{
    public string Name { get; set; }
}

internal static class IHasNameExtensions
{
    public static void WriteNameTo(this IHasName self, CodeWriter writer)
    {
        writer.Write(CodeWriter.SanitizeIdentifier(self.Name));
    }
}
