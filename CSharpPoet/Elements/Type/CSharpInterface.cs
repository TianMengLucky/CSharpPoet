namespace CSharpPoet;

public class CSharpInterface : CSharpType
{
    public CSharpInterface(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpInterface(string name) : base(name)
    {
    }

    public override string Type => "interface";
}
