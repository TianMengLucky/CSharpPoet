namespace CSharpPoet;

public class CSharpNamespace : CSharpMember<CSharpFile.IMember>, CSharpFile.IMember
{
    public CSharpNamespace(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public IList<CSharpUsing> Usings { get; set; } = new List<CSharpUsing>();

    /// <inheritdoc />
    public override void WriteTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteLine($"namespace {Name}");
        using (writer.Block())
        {
            WriteUsingsTo(writer);

            writer.WriteMembers(Members);
        }
    }

    public void WriteUsingsTo(CodeWriter writer)
    {
        if (writer == null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        if (Usings.Any())
        {
            writer.WriteMembers(Usings);
            writer.WriteLine();
        }
    }
}
