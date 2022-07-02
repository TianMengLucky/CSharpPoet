namespace CSharpPoet;

public class CSharpRecord : CSharpType
{
    public override string Type => "record";

    public CSharpRecord(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpRecord(string name) : base(name)
    {
    }

    // TODO primary constructor member
}
