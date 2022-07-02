namespace CSharpPoet;

/// <summary>
/// Represents a C# type like enum or class-likes (class, interface, struct, record).  
/// </summary>
public abstract class CSharpBaseType<TMember> : CSharpMember<TMember>, CSharpFile.IMember, CSharpType.IMember, IHasSeparator where TMember : ICSharpMember
{
    string IHasSeparator.Separator => "\n";

    public IList<CSharpAttribute> Attributes { get; init; } = new List<CSharpAttribute>();

    public Visibility Visibility { get; set; }

    public string Name { get; set; }

    public abstract string Type { get; }

    #region Modifiers

    public bool IsStatic { get; set; }

    public bool IsUnsafe { get; set; }

    public bool IsPartial { get; set; }

    #endregion

    protected CSharpBaseType(Visibility visibility, string name)
    {
        Visibility = visibility;
        Name = name;
    }

    protected CSharpBaseType(string name) : this(Visibility.Public, name)
    {
    }

    protected abstract bool HasExtends { get; }
    protected abstract void WriteExtendsTo(CodeWriter writer);

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
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
        if (IsUnsafe) writer.Write("unsafe ");
        if (IsPartial) writer.Write("partial ");

        writer.Write(Type);
        writer.Write(' ');

        writer.Write(CodeWriter.SanitizeIdentifier(Name));

        if (HasExtends)
        {
            writer.Write(" : ");
            WriteExtendsTo(writer);
        }

        writer.WriteLine();

        using (writer.Block())
        {
            writer.WriteMembers(Members);
        }
    }
}
