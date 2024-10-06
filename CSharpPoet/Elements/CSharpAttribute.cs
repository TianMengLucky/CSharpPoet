namespace CSharpPoet;

/// <summary>
///     Represents an attribute that is associated to a member.
/// </summary>
public class CSharpAttribute : CSharpMember<CSharpAttribute.IArgument>
{
    /// <summary>
    ///     Initializes an empty attribute.
    /// </summary>
    /// <param name="name">The name of the custom attribute.</param>
    public CSharpAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     Gets or sets the name.
    /// </summary>
    public string Name { get; set; }

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

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
    ///     Represents an argument of <see cref="CSharpAttribute" />.
    /// </summary>
    public interface IArgument : ICSharpMember
    {
    }

    /// <summary>
    ///     Represents a parameter of an attribute constructor.
    /// </summary>
    public class Parameter : IArgument, IHasSeparator
    {
        public Parameter(string value)
        {
            Value = value;
        }

        public Parameter(string? name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <inheritdoc />
        public void WriteTo(CodeWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (Name != null)
            {
                writer.Write(Name);
                writer.Write(": ");
            }

            writer.Write(Value);
        }

        string IHasSeparator.Separator => ", ";

        public static implicit operator Parameter(string value)
        {
            return new Parameter(value);
        }
    }

    /// <summary>
    ///     Represents a property assignment of an attribute constructor.
    /// </summary>
    public class Property : IArgument, IHasSeparator
    {
        public Property(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <inheritdoc />
        public void WriteTo(CodeWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(Name);
            writer.Write(" = ");
            writer.Write(Value);
        }

        string IHasSeparator.Separator => ", ";
    }
}
