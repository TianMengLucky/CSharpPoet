namespace CSharpPoet.Tests;

[UsesVerify]
public class CodeWriterTest
{
    [Fact]
    public Task Test()
    {
        using var stringWriter = new StringWriter();
        using var codeWriter = new CodeWriter(stringWriter);

        codeWriter.WriteLine("namespace TestNamespace;");
        codeWriter.WriteLine();

        codeWriter.WriteLine("public class TestClass");
        using (codeWriter.Block())
        {
            codeWriter.WriteLine();
            codeWriter.WriteLine("public static readonly int TestField = 1;");
            codeWriter.WriteLine();
        }

        return Verify(stringWriter.ToString());
    }
}
