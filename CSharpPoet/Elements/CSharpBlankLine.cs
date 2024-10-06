namespace CSharpPoet;

/// <summary>
///     Represents a blank line.
/// </summary>
public class CSharpBlankLine : CSharpType.IMember
{
    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteLine();
    }
}
