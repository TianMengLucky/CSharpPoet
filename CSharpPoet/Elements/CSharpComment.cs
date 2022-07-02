namespace CSharpPoet;

/// <summary>
/// Represents a C# comment.
/// </summary>
public class CSharpComment : CSharpFile.IMember, CSharpType.IMember
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public Action<CodeWriter> Value { get; set; }

    public CSharpComment(Action<CodeWriter> value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public virtual void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        writer.Write("// ");
        Value(writer);
    }
}

/// Represents a multiline C# comment.
public class CSharpMultiLineComment : CSharpComment
{
    public CSharpMultiLineComment(Action<CodeWriter> value) : base(value)
    {
    }

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        using (writer.MultilineComment())
        {
            Value(writer);
        }
    }
}
