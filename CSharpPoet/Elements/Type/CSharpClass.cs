namespace CSharpPoet;

public class CSharpClass : CSharpType
{
    public override string Type => "class";

    public CSharpClass(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpClass(string name) : base(name)
    {
    }
}
