namespace CSharpPoet;

/// <summary>
/// Represents a member that has attributes.
/// </summary>
public interface IHasAttributes : ICSharpMember
{
    /// <summary>
    /// Gets or sets a list of attributes assigned to this member.
    /// </summary>
    public IList<CSharpAttribute> Attributes { get; set; }
}

/// <summary>
/// Represents an attribute that is associated to a member.
/// </summary>
public class CSharpAttribute : CSharpMember<CSharpAttribute.IArgument>
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initializes an empty attribute.
    /// </summary>
    /// <param name="name">The name of the custom attribute.</param>
    public CSharpAttribute(string name)
    {
        Name = name;
    }

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        writer.Write('[');

        writer.Write(CodeWriter.SanitizeIdentifier(Name));

        if (Members.Any())
        {
            writer.Write('(');
            writer.WriteMembers(Members);
            writer.Write(')');
        }

        writer.Write(']');
    }

    /// <summary>
    /// Represents an argument of <see cref="CSharpAttribute"/>.
    /// </summary>
    public interface IArgument : ICSharpMember
    {
    }

    /// <summary>
    /// Represents a parameter of an attribute constructor.
    /// </summary>
    public class Parameter : IArgument, IHasSeparator
    {
        string IHasSeparator.Separator => ", ";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        public Parameter(string value)
        {
            Value = value;
        }

        public Parameter(string? name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public void WriteTo(CodeWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (Name != null)
            {
                writer.Write(Name);
                writer.Write(": ");
            }

            writer.Write(Value);
        }

        public static implicit operator Parameter(string value)
        {
            return new Parameter(value);
        }
    }

    /// <summary>
    /// Represents a property assignment of an attribute constructor.
    /// </summary>
    public class Property : IArgument, IHasSeparator
    {
        string IHasSeparator.Separator => ", ";

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <inheritdoc />
        public void WriteTo(CodeWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            writer.Write(Name);
            writer.Write(" = ");
            writer.Write(Value);
        }
    }
}
