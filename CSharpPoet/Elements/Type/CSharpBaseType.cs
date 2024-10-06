using CSharpPoet.Traits;

namespace CSharpPoet;

/// <summary>
///     Represents a C# type like enum or class-likes (class, interface, struct, record).
/// </summary>
public abstract class CSharpBaseType<TMember>(Visibility visibility, string name) : CSharpMember<TMember>,
    CSharpFile.IMember, CSharpType.IMember, IHasSeparator,
    IHasXmlComment,
    IHasAttributes,
    IHasVisibility,
    IHasModifiers,
    IHasName
    where TMember : ICSharpMember
{
    protected CSharpBaseType(string name) : this(CSharpPoet.Visibility.Public, name)
    {
    }

    public abstract string Type { get; }

    protected abstract bool HasExtends { get; }
    string IHasSeparator.Separator => "\n";

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        this.WriteXmlCommentTo(writer);
        this.WriteAttributesTo(writer);

        this.WriteVisibilityTo(writer);

        this.WriteModifiersTo(writer);

        writer.Write(Type);
        writer.Write(' ');

        this.WriteNameTo(writer);

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

    protected abstract void WriteExtendsTo(CodeWriter writer);

    #region Traits

    public Action<CodeWriter>? XmlComment { get; set; }
    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();
    public Visibility? Visibility { get; set; } = visibility;
    public Modifiers Modifiers { get; set; }
    public string Name { get; set; } = name;

    #endregion

    #region Modifiers

    public bool IsStatic
    {
        get => this.HasModifier(Modifiers.Static);
        set => this.SetModifier(Modifiers.Static, value);
    }

    public bool IsNew { get => this.HasModifier(Modifiers.New); set => this.SetModifier(Modifiers.New, value); }

    public bool IsAbstract
    {
        get => this.HasModifier(Modifiers.Abstract);
        set => this.SetModifier(Modifiers.Abstract, value);
    }

    public bool IsSealed
    {
        get => this.HasModifier(Modifiers.Sealed);
        set => this.SetModifier(Modifiers.Sealed, value);
    }

    public bool IsReadonly
    {
        get => this.HasModifier(Modifiers.Readonly);
        set => this.SetModifier(Modifiers.Readonly, value);
    }

    public bool IsUnsafe
    {
        get => this.HasModifier(Modifiers.Unsafe);
        set => this.SetModifier(Modifiers.Unsafe, value);
    }

    public bool IsPartial
    {
        get => this.HasModifier(Modifiers.Partial);
        set => this.SetModifier(Modifiers.Partial, value);
    }

    #endregion
}
