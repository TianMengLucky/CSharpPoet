namespace CSharpPoet;

public class CSharpInterface : CSharpType
{
    public override string Type => "interface";

    public CSharpInterface(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpInterface(string name) : base(name)
    {
    }
}
