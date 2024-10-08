namespace CSharpPoet;

public abstract class CSharpType : CSharpBaseType<CSharpType.IMember>
{
    protected CSharpType(Visibility visibility, string name) : base(visibility, name)
    {
    }

    protected CSharpType(string name) : base(name)
    {
    }

    public IList<string> Extends { get; set; } = [];

    protected override bool HasExtends => Extends.Any();

    protected override void WriteExtendsTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        var first = true;

        foreach (var name in Extends)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                writer.Write(", ");
            }

            writer.Write(name);
        }
    }

    /// <summary>
    ///     Represents a member of <see cref="CSharpType" />.
    /// </summary>
    public interface IMember : ICSharpMember
    {
    }
}
