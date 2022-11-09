using CSharpPoet.Traits;

namespace CSharpPoet;

public class CSharpField : CSharpType.IMember,
    IHasXmlComment,
    IHasAttributes,
    IHasVisibility,
    IHasModifiers,
    IHasType,
    IHasName,
    IHasDefaultValue
{
    #region Traits

    public Action<CodeWriter>? XmlComment { get; set; }
    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();
    public Visibility? Visibility { get; set; }
    public Modifiers Modifiers { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public string? DefaultValue { get; set; }

    #endregion

    #region Modifiers

    public bool IsStatic { get => this.HasModifier(Modifiers.Static); set => this.SetModifier(Modifiers.Static, value); }
    public bool IsNew { get => this.HasModifier(Modifiers.New); set => this.SetModifier(Modifiers.New, value); }
    public bool IsReadonly { get => this.HasModifier(Modifiers.Readonly); set => this.SetModifier(Modifiers.Readonly, value); }
    public bool IsUnsafe { get => this.HasModifier(Modifiers.Unsafe); set => this.SetModifier(Modifiers.Unsafe, value); }
    public bool IsRequired { get => this.HasModifier(Modifiers.Required); set => this.SetModifier(Modifiers.Required, value); }
    public bool IsVolatile { get => this.HasModifier(Modifiers.Volatile); set => this.SetModifier(Modifiers.Volatile, value); }
    public bool IsConst { get => this.HasModifier(Modifiers.Const); set => this.SetModifier(Modifiers.Const, value); }

    #endregion

    public CSharpField(Visibility visibility, string type, string name)
    {
        Visibility = visibility;
        Type = type;
        Name = name;
    }

    public CSharpField(string type, string name) : this(CSharpPoet.Visibility.Private, type, name)
    {
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        this.WriteXmlCommentTo(writer);
        this.WriteAttributesTo(writer);

        this.WriteVisibilityTo(writer);

        this.WriteModifiersTo(writer);

        this.WriteTypeTo(writer);

        this.WriteNameTo(writer);

        this.WriteDefaultValueTo(writer);

        writer.WriteLine(';');
    }
}
