namespace CSharpPoet;

public class CSharpStruct : CSharpType
{
    public override string Type => "struct";

    public CSharpStruct(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpStruct(string name) : base(name)
    {
    }
}
