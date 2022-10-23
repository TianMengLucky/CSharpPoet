namespace CSharpPoet;

public class CSharpField : CSharpType.IMember
{
    public Visibility Visibility { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }

    public bool IsStatic { get; set; }
    public bool IsReadonly { get; set; }
    public bool IsConst { get; set; }

    public string? DefaultValue { get; set; }

    public CSharpField(Visibility visibility, string type, string name)
    {
        Visibility = visibility;
        Type = type;
        Name = name;
    }

    public CSharpField(string type, string name) : this(Visibility.Private, type, name)
    {
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        writer.Write(Visibility);
        writer.Write(' ');

        if (IsStatic) writer.Write("static ");
        if (IsReadonly) writer.Write("readonly ");
        if (IsConst) writer.Write("const ");

        writer.Write(Type);
        writer.Write(' ');

        writer.Write(CodeWriter.SanitizeIdentifier(Name));

        if (DefaultValue != null)
        {
            writer.Write(" = ");
            writer.Write(DefaultValue);
        }

        writer.WriteLine(';');
    }
}
