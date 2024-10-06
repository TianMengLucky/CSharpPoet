namespace CSharpPoet;

public class CSharpRecord : CSharpType
{
    public CSharpRecord(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpRecord(string name) : base(name)
    {
    }

    public override string Type => "record";

    // TODO primary constructor member
}
