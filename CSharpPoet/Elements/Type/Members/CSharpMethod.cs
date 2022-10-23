namespace CSharpPoet;

public class CSharpMethod : CSharpType.IMember, IHasAttributes, IHasSeparator
{
    string IHasSeparator.Separator => "\n";

    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();

    public Visibility Visibility { get; set; }
    public string ReturnType { get; set; }
    public string Name { get; set; }

    public IList<CSharpParameter> Parameters { get; set; } = new List<CSharpParameter>();

    public bool IsStatic { get; set; }
    public bool IsExtern { get; set; }

    public Action<CodeWriter>? Body { get; set; }

    public CSharpMethod(Visibility visibility, string returnType, string name)
    {
        Visibility = visibility;
        ReturnType = returnType;
        Name = name;
    }

    public CSharpMethod(string returnType, string name) : this(Visibility.Public, returnType, name)
    {
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        foreach (var attribute in Attributes)
        {
            attribute.WriteTo(writer);
            writer.WriteLine();
        }

        writer.Write(Visibility);
        writer.Write(' ');

        if (IsStatic) writer.Write("static ");
        if (IsExtern) writer.Write("extern ");

        writer.Write(ReturnType);
        writer.Write(' ');

        writer.Write(CodeWriter.SanitizeIdentifier(Name));

        writer.Write('(');
        writer.WriteMembers(Parameters);
        writer.Write(')');

        if (Body == null)
        {
            writer.WriteLine(";");
        }
        else
        {
            writer.WriteLine();
            using (writer.Block())
            {
                Body.Invoke(writer);
            }
        }
    }
}

public class CSharpParameter : ICSharpMember, IHasAttributes, IHasSeparator
{
    string IHasSeparator.Separator => ", ";

    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();

    public string Type { get; set; }
    public string Name { get; set; }

    public string? DefaultValue { get; set; }

    public CSharpParameter(string type, string name)
    {
        Type = type;
        Name = name;
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        foreach (var attribute in Attributes)
        {
            attribute.WriteTo(writer);
            writer.Write(" ");
        }

        writer.Write(Type);
        writer.Write(' ');

        writer.Write(CodeWriter.SanitizeIdentifier(Name));

        if (DefaultValue != null)
        {
            writer.Write(" = ");
            writer.Write(DefaultValue);
        }
    }
}
