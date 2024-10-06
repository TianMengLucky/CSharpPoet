namespace CSharpPoet;

public class CSharpClass : CSharpType
{
    public CSharpClass(Visibility visibility, string name) : base(visibility, name)
    {
    }

    public CSharpClass(string name) : base(name)
    {
    }

    public override string Type => "class";

    public bool HasGenerics { get; set; } = false;
    public string[] GenericsNames { get; set; }
}
