using CSharpPoet.Traits;

namespace CSharpPoet;

public class CSharpEnum : CSharpBaseType<CSharpEnum.Member>
{
    public CSharpEnum(Visibility visibility, string name,
        CSharpEnumUnderlyingType underlyingType = CSharpEnumUnderlyingType.Int) : base(visibility, name)
    {
        UnderlyingType = underlyingType;
    }

    public CSharpEnum(string name, CSharpEnumUnderlyingType underlyingType = CSharpEnumUnderlyingType.Int) : this(
        CSharpPoet.Visibility.Public, name, underlyingType)
    {
    }

    public override string Type => "enum";

    public CSharpEnumUnderlyingType UnderlyingType { get; set; }

    protected override bool HasExtends => UnderlyingType != CSharpEnumUnderlyingType.Int;

    protected override void WriteExtendsTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

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
            _ => throw new ArgumentOutOfRangeException(nameof(UnderlyingType), UnderlyingType,
                "Invalid CSharpEnumUnderlyingType value")
        });
    }

    public class Member : ICSharpMember,
        IHasXmlComment,
        IHasAttributes,
        IHasName
    {
        public Member(string name, string? value = null)
        {
            Name = name;
            Value = value;
        }

        public string? Value { get; set; }

        /// <inheritdoc />
        public void WriteTo(CodeWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

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

        public IList<CSharpAttribute> Attributes { get; set; } = new List<CSharpAttribute>();

        public string Name { get; set; }
        public Action<CodeWriter>? XmlComment { get; set; }
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
    UnsignedLong
}
