using CSharpPoet.Traits;

namespace CSharpPoet;

public class CSharpEnum : CSharpBaseType<CSharpEnum.Member>
{
    public override string Type => "enum";

    public CSharpEnumUnderlyingType UnderlyingType { get; set; }

    public CSharpEnum(Visibility visibility, string name, CSharpEnumUnderlyingType underlyingType = CSharpEnumUnderlyingType.Int) : base(visibility, name)
    {
        UnderlyingType = underlyingType;
    }

    public CSharpEnum(string name, CSharpEnumUnderlyingType underlyingType = CSharpEnumUnderlyingType.Int) : this(CSharpPoet.Visibility.Public, name, underlyingType)
    {
    }

    protected override bool HasExtends => UnderlyingType != CSharpEnumUnderlyingType.Int;

    protected override void WriteExtendsTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        writer.Write(UnderlyingType switch
        {
            CSharpEnumUnderlyingType.Byte => "byte",
            CSharpEnumUnderlyingType.SignedByte => "sbyte",
            CSharpEnumUnderlyingType.Short => "short",
            CSharpEnumUnderlyingType.UnsignedShort => "ushort",
            CSharpEnumUnderlyingType.Int => "int",
            CSharpEnumUnderlyingType.UnsignedInt => "uint",
            CSharpEnumUnderlyingType.Long => "long",
            CSharpEnumUnderlyingType.UnsignedLong => "ulong",
            _ => throw new ArgumentOutOfRangeException(nameof(UnderlyingType), UnderlyingType, "Invalid CSharpEnumUnderlyingType value"),
        });
    }

    public class Member : ICSharpMember,
        IHasXmlComment,
        IHasAttributes,
        IHasName
    {
        public Action<CodeWriter>? XmlComment { get; set; }

        public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();

        public string Name { get; set; }

        public string? Value { get; set; }

        public Member(string name, string? value = null)
        {
            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public void WriteTo(CodeWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            this.WriteXmlCommentTo(writer);
            this.WriteAttributesTo(writer);

            this.WriteNameTo(writer);

            if (Value != null)
            {
                writer.Write(" = ");
                writer.Write(Value);
            }

            writer.WriteLine(',');
        }
    }
}

public enum CSharpEnumUnderlyingType
{
    Byte,
    SignedByte,
    Short,
    UnsignedShort,
    Int,
    UnsignedInt,
    Long,
    UnsignedLong,
}
