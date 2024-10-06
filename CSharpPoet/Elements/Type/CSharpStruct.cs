namespace CSharpPoet;

public class CSharpStruct : CSharpType
{
    public CSharpStruct(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpStruct(string name) : base(name)
    {
    }

    public override string Type => "struct";
}
