namespace CSharpPoet;

public class CSharpUsing : ICSharpMember
{
    public CSharpUsing(string target)
    {
        Target = target;
    }

    public CSharpUsing(string? alias, string target)
    {
        Alias = alias;
        Target = target;
    }

    public string Target { get; set; }

    public string? Alias { get; set; }

    public bool IsGlobal { get; set; }

    public bool IsStatic { get; set; }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (IsGlobal)
        {
            writer.Write("global ");
        }

        writer.Write("using ");

        if (IsStatic)
        {
            writer.Write("static ");
        }

        if (Alias != null)
        {
            writer.Write(Alias);
            writer.Write(" = ");
        }

        writer.Write(Target);
        writer.WriteLine(";");
    }

    public static implicit operator CSharpUsing(string target)
    {
        return new CSharpUsing(target);
    }
}
