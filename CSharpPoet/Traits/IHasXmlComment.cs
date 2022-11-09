namespace CSharpPoet.Traits;

public interface IHasXmlComment
{
    Action<CodeWriter>? XmlComment { get; set; }
}

internal static class IHasXmlCommentExtensions
{
    public static void WriteXmlCommentTo(this IHasXmlComment self, CodeWriter writer)
    {
        if (self.XmlComment != null)
        {
            using (writer.XmlComment()) self.XmlComment(writer);
        }
    }
}
