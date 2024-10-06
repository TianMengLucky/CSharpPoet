using CSharpPoet.Traits;

namespace CSharpPoet;

public class NumberCSharpField<T> : CSharpField where T : struct, IFormattable
{
    public static readonly Dictionary<Type, string> TypeNames = new()
    {
        { typeof(int), "int" },
        { typeof(long), "long" },
        { typeof(decimal), "decimal" },
        { typeof(sbyte), "sbyte" },
        { typeof(ulong), "ulong" },
        { typeof(ushort), "ushort" },
        { typeof(uint), "uint" },
        { typeof(short), "short" },
        { typeof(byte), "byte" },
        { typeof(float), "float" },
        { typeof(double), "double" }
    };

    public NumberCSharpField(string name, T value) : base(GetTypeName<T>(), name)
    {
        DefaultValue = new NumberValue<T>(value);
    }

    public static string GetTypeName<TType>()
    {
        if (TypeNames.TryGetValue(typeof(TType), out var value))
        {
            return value;
        }

        throw new Exception($"No Has Number Type {typeof(T)}");
    }
}

public class CSharpField(Visibility visibility, string type, string name) : CSharpType.IMember,
    IHasXmlComment,
    IHasAttributes,
    IHasVisibility,
    IHasModifiers,
    IHasType,
    IHasName,
    IHasDefaultValue
{
    public CSharpField(string type, string name) : this(CSharpPoet.Visibility.Private, type, name)
    {
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        this.WriteXmlCommentTo(writer);
        this.WriteAttributesTo(writer);

        this.WriteVisibilityTo(writer);

        this.WriteModifiersTo(writer);

        this.WriteTypeTo(writer);

        this.WriteNameTo(writer);

        this.WriteDefaultValueTo(writer);

        writer.WriteLine(';');
    }

    #region Traits

    public Action<CodeWriter>? XmlComment { get; set; }
    public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();
    public Visibility? Visibility { get; set; } = visibility;
    public Modifiers Modifiers { get; set; }
    public string Type { get; set; } = type;
    public string Name { get; set; } = name;
    public Value? DefaultValue { get; set; }

    #endregion

    #region Modifiers

    public bool IsStatic
    {
        get => this.HasModifier(Modifiers.Static);
        set => this.SetModifier(Modifiers.Static, value);
    }

    public bool IsNew { get => this.HasModifier(Modifiers.New); set => this.SetModifier(Modifiers.New, value); }

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

    public bool IsRequired
    {
        get => this.HasModifier(Modifiers.Required);
        set => this.SetModifier(Modifiers.Required, value);
    }

    public bool IsVolatile
    {
        get => this.HasModifier(Modifiers.Volatile);
        set => this.SetModifier(Modifiers.Volatile, value);
    }

    public bool IsConst { get => this.HasModifier(Modifiers.Const); set => this.SetModifier(Modifiers.Const, value); }

    #endregion
}
