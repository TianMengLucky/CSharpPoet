using System.Collections;

namespace CSharpPoet;

/// <summary>
/// Represents all write-able C# member.
/// </summary>
public interface ICSharpMember
{
    /// <summary>
    /// Writes the the member to <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The <see cref="CodeWriter"/> to write to.</param>
    void WriteTo(CodeWriter writer);
}

/// <summary>
/// Member that has to be separated from others using <see cref="Separator"/>.
/// Used by <see cref="CodeWriter.WriteMembers{T}"/>.
/// </summary>
public interface IHasSeparator : ICSharpMember
{
    /// <summary>
    /// Gets the separator.
    /// </summary>
    string? Separator { get; }
}

/// <summary>
/// Utility extensions for <see cref="ICSharpMember"/>.
/// </summary>
public static class CSharpMemberExtensions
{
    /// <summary>
    /// Writes the <paramref name="member"/> to <paramref name="writer"/>.
    /// </summary>
    /// <param name="member">The member to be written.</param>
    /// <param name="writer">The writer to be written to.</param>
    public static void WriteTo(this ICSharpMember member, TextWriter writer)
    {
        if (member == null) throw new ArgumentNullException(nameof(member));
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        using var codeWriter = new CodeWriter(writer);
        member.WriteTo(codeWriter);
    }

    /// <summary>
    /// Writes the <paramref name="member"/> to <paramref name="filePath"/>
    /// </summary>
    /// <param name="member">The member to be written.</param>
    /// <param name="filePath">The path of the file to be written to.</param>
    public static void WriteTo(this ICSharpMember member, string filePath)
    {
        new FileInfo(filePath).Directory?.Create();
        using var streamWriter = new StreamWriter(filePath, false);
        member.WriteTo(streamWriter);
    }
}

/// <summary>
/// Represents a C# member that has children members.
/// </summary>
/// <typeparam name="TMember">The type of children members.</typeparam>
public abstract class CSharpMember<TMember> : ICSharpMember, IEnumerable<TMember> where TMember : ICSharpMember
{
    /// <summary>
    /// Gets or sets the children members.
    /// </summary>
    public IList<TMember> Members { get; init; } = new List<TMember>();

    /// <inheritdoc />
    public IEnumerator<TMember> GetEnumerator()
    {
        return Members.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Adds a member.
    /// </summary>
    /// <param name="member">The member to add.</param>
    public void Add(TMember member)
    {
        Members.Add(member);
    }

    /// <inheritdoc />
    public abstract void WriteTo(CodeWriter writer);

    /// <inheritdoc />
    public override string ToString()
    {
        using var stringWriter = new StringWriter();
        this.WriteTo(stringWriter);

        return stringWriter.ToString();
    }
}
