using CSharpPoet.Traits;

namespace CSharpPoet;

public class CSharpProperty : CSharpType.IMember, IHasSeparator,
    IHasXmlComment,
    IHasAttributes,
    IHasVisibility,
    IHasModifiers,
    IHasType,
    IHasName,
    IHasDefaultValue
{
    string? IHasSeparator.Separator => IsMultiline ? "\n" : null;

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
    public bool IsExtern { get => this.HasModifier(Modifiers.Extern); set => this.SetModifier(Modifiers.Extern, value); }
    public bool IsNew { get => this.HasModifier(Modifiers.New); set => this.SetModifier(Modifiers.New, value); }
    public bool IsVirtual { get => this.HasModifier(Modifiers.Virtual); set => this.SetModifier(Modifiers.Virtual, value); }
    public bool IsAbstract { get => this.HasModifier(Modifiers.Abstract); set => this.SetModifier(Modifiers.Abstract, value); }
    public bool IsSealed { get => this.HasModifier(Modifiers.Sealed); set => this.SetModifier(Modifiers.Sealed, value); }
    public bool IsOverride { get => this.HasModifier(Modifiers.Override); set => this.SetModifier(Modifiers.Override, value); }
    public bool IsUnsafe { get => this.HasModifier(Modifiers.Unsafe); set => this.SetModifier(Modifiers.Unsafe, value); }
    public bool IsRequired { get => this.HasModifier(Modifiers.Required); set => this.SetModifier(Modifiers.Required, value); }
    public bool IsPartial { get => this.HasModifier(Modifiers.Partial); set => this.SetModifier(Modifiers.Partial, value); }

    #endregion

    public Accessor? Getter { get; set; }
    public Accessor? Setter { get; set; }

    public bool IsInitOnly { get; set; }

    public bool IsMultiline => Getter is { BodyType: BodyType.Block } || Setter is { BodyType: BodyType.Block };

    public CSharpProperty(Visibility? visibility, string type, string name)
    {
        Visibility = visibility;
        Type = type;
        Name = name;
    }

    public CSharpProperty(string type, string name) : this(CSharpPoet.Visibility.Public, type, name)
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

        writer.Write(' ');

        if (Getter is { Body: { }, BodyType: BodyType.Expression } && Setter == null)
        {
            writer.Write("=> ");
            Getter.Body(writer);
            writer.WriteLine();
        }
        else
        {
            CodeWriter.BlockScope? blockScope = null;

            if (IsMultiline)
            {
                writer.WriteLine();
                blockScope = writer.Block();
            }
            else
            {
                writer.Write('{');
            }

            if (Getter == null && Setter == null)
            {
                writer.Write(' ');
            }
            else
            {
                if (Getter != null)
                {
                    if (blockScope == null) writer.Write(" ");
                    Getter.WriteTo(writer, "get");

                    if (blockScope == null && Setter == null) writer.Write(" ");
                    else if (blockScope != null && Setter != null) writer.WriteLine();
                }

                if (Setter != null)
                {
                    if (blockScope == null) writer.Write(" ");
                    Setter.WriteTo(writer, IsInitOnly ? "init" : "set");
                    if (blockScope == null) writer.Write(" ");
                }
            }

            if (blockScope != null)
            {
                blockScope.Value.Dispose();
            }
            else
            {
                writer.Write('}');

                if (DefaultValue != null)
                {
                    this.WriteDefaultValueTo(writer);
                    writer.Write(';');
                }

                writer.WriteLine();
            }
        }
    }

    public class Accessor : IHasVisibility
    {
        #region Traits

        public Visibility? Visibility { get; set; }

        #endregion

        public BodyType BodyType { get; set; } = BodyType.Expression;

        public Action<CodeWriter>? Body { get; set; }

        public Accessor()
        {
        }

        public Accessor(Visibility? visibility)
        {
            Visibility = visibility;
        }

        public void WriteTo(CodeWriter writer, string name)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            this.WriteVisibilityTo(writer);

            writer.Write(name);

            if (Body != null)
            {
                if (BodyType == BodyType.Expression)
                {
                    writer.Write(" => ");
                    Body(writer);
                }
                else
                {
                    writer.WriteLine();
                    using (writer.Block())
                    {
                        Body(writer);
                    }
                }
            }
            else
            {
                writer.Write(';');
            }
        }
    }
}
