namespace CSharpPoet.Tests;

public class CodeWriterTest
{
    [Fact]
    public void Test()
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

        Assert.Equal(
            "namespace TestNamespace;\n" +
            "\n" +
            "public class TestClass\n" +
            "{\n" +
            "\n" +
            "    public static readonly int TestField = 1;\n" +
            "\n" +
            "}\n", stringWriter.ToString());
    }
}
