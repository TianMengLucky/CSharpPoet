using CSharpPoet.Traits;

namespace CSharpPoet;

/// <summary>
/// Represents a C# type like enum or class-likes (class, interface, struct, record).  
/// </summary>
public abstract class CSharpBaseType<TMember> : CSharpMember<TMember>, CSharpFile.IMember, CSharpType.IMember, IHasSeparator,
    IHasXmlComment,
    IHasAttributes,
    IHasVisibility,
    IHasModifiers,
    IHasName
    where TMember : ICSharpMember
{
    string IHasSeparator.Separator => "\n";

    #region Traits

    public Action<CodeWriter>? XmlComment { get; set; }
    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();
    public Visibility? Visibility { get; set; }
    public Modifiers Modifiers { get; set; }
    public string Name { get; set; }

    #endregion

    #region Modifiers

    public bool IsStatic { get => this.HasModifier(Modifiers.Static); set => this.SetModifier(Modifiers.Static, value); }
    public bool IsNew { get => this.HasModifier(Modifiers.New); set => this.SetModifier(Modifiers.New, value); }
    public bool IsAbstract { get => this.HasModifier(Modifiers.Abstract); set => this.SetModifier(Modifiers.Abstract, value); }
    public bool IsSealed { get => this.HasModifier(Modifiers.Sealed); set => this.SetModifier(Modifiers.Sealed, value); }
    public bool IsReadonly { get => this.HasModifier(Modifiers.Readonly); set => this.SetModifier(Modifiers.Readonly, value); }
    public bool IsUnsafe { get => this.HasModifier(Modifiers.Unsafe); set => this.SetModifier(Modifiers.Unsafe, value); }
    public bool IsPartial { get => this.HasModifier(Modifiers.Partial); set => this.SetModifier(Modifiers.Partial, value); }

    #endregion

    public abstract string Type { get; }

    protected CSharpBaseType(Visibility visibility, string name)
    {
        Visibility = visibility;
        Name = name;
    }

    protected CSharpBaseType(string name) : this(CSharpPoet.Visibility.Public, name)
    {
    }

    protected abstract bool HasExtends { get; }
    protected abstract void WriteExtendsTo(CodeWriter writer);

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

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
}
