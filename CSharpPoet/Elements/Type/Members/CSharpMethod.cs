using CSharpPoet.Traits;

namespace CSharpPoet;

public class CSharpMethod : CSharpType.IMember, IHasSeparator,
    IHasXmlComment,
    IHasAttributes,
    IHasVisibility,
    IHasModifiers,
    IHasName
{
    string IHasSeparator.Separator => "\n";

    #region Traits

    public Action<CodeWriter>? XmlComment { get; set; }
    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();
    public Visibility? Visibility { get; set; }
    public Modifiers Modifiers { get; set; }
    public string Name { get; set; }
    public string? DefaultValue { get; set; }

    #endregion

    #region Modifiers

    public bool IsStatic { get => this.HasModifier(Modifiers.Static); set => this.SetModifier(Modifiers.Static, value); }
    public bool IsExtern { get => this.HasModifier(Modifiers.Extern); set => this.SetModifier(Modifiers.Extern, value); }
    public bool IsNew { get => this.HasModifier(Modifiers.New); set => this.SetModifier(Modifiers.New, value); }
    public bool IsVirtual { get => this.HasModifier(Modifiers.Virtual); set => this.SetModifier(Modifiers.Virtual, value); }
    public bool IsAbstract { get => this.HasModifier(Modifiers.Abstract); set => this.SetModifier(Modifiers.Abstract, value); }
    public bool IsSealed { get => this.HasModifier(Modifiers.Sealed); set => this.SetModifier(Modifiers.Sealed, value); }
    public bool IsOverride { get => this.HasModifier(Modifiers.Override); set => this.SetModifier(Modifiers.Override, value); }
    public bool IsUnsafe { get => this.HasModifier(Modifiers.Unsafe); set => this.SetModifier(Modifiers.Unsafe, value); }
    public bool IsAsync { get => this.HasModifier(Modifiers.Async); set => this.SetModifier(Modifiers.Async, value); }
    public bool IsPartial { get => this.HasModifier(Modifiers.Partial); set => this.SetModifier(Modifiers.Partial, value); }

    #endregion

    public string ReturnType { get; set; }

    public IList<CSharpParameter> Parameters { get; set; } = new List<CSharpParameter>();

    public BodyType BodyType { get; set; } = BodyType.Block;
    public Action<CodeWriter>? Body { get; set; }

    public CSharpMethod(Visibility visibility, string returnType, string name)
    {
        Visibility = visibility;
        ReturnType = returnType;
        Name = name;
    }

    public CSharpMethod(string returnType, string name) : this(CSharpPoet.Visibility.Public, returnType, name)
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

        if (Name is ".ctor" or ".cctor")
        {
            writer.Write(ReturnType);
        }
        else
        {
            writer.Write(ReturnType);
            writer.Write(' ');

            this.WriteNameTo(writer);
        }

        writer.Write('(');
        writer.WriteMembers(Parameters);
        writer.Write(')');

        if (Body == null)
        {
            writer.WriteLine(";");
        }
        else
        {
            if (BodyType == BodyType.Block)
            {
                writer.WriteLine();
                using (writer.Block())
                {
                    Body(writer);
                }
            }
            else if (BodyType == BodyType.Expression)
            {
                writer.Write(" => ");
                Body(writer);
            }
        }
    }
}

public class CSharpParameter : ICSharpMember, IHasSeparator,
    IHasAttributes,
    IHasType,
    IHasName,
    IHasDefaultValue
{
    string IHasSeparator.Separator => ", ";

    #region Traits

    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();
    public string Type { get; set; }
    public string Name { get; set; }
    public string? DefaultValue { get; set; }

    #endregion

    public CSharpParameter(string type, string name)
    {
        Type = type;
        Name = name;
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        this.WriteAttributesTo(writer, " ");

        this.WriteTypeTo(writer);

        this.WriteNameTo(writer);

        this.WriteDefaultValueTo(writer);
    }
}
