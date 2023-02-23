namespace CSharpPoet.Tests;

[UsesVerify]
public class CSharpTest
{
    [Fact]
    public Task FileTest()
    {
        var file = new CSharpFile(new CSharpNamespace("TestNamespace")
        {
            Usings =
            {
                new CSharpUsing("NamespaceB"),
            },
        })
        {
            Usings =
            {
                new CSharpUsing("NamespaceA"),
                new CSharpUsing("NamespaceA.A") { IsGlobal = true, IsStatic = true },
                new CSharpUsing("AliasA", "NamespaceA"),
            },
        };

        return Verify(file.ToString());
    }

    [Fact]
    public Task EnumTest()
    {
        var @enum = new CSharpEnum("TestEnum", underlyingType: CSharpEnumUnderlyingType.SignedByte)
        {
            new("A"),
            new("B"),
            new("C", "10"),
        };

        return Verify(@enum.ToString());
    }

    [Fact]
    public Task StructTest()
    {
        var @struct = new CSharpStruct("TestStruct");

        return Verify(@struct.ToString());
    }

    [Fact]
    public Task RecordTest()
    {
        var record = new CSharpRecord("TestRecord");

        return Verify(record.ToString());
    }

    [Fact]
    public Task ExtendsTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            Extends = { "ITestInterface" },
        };

        return Verify(@class.ToString());
    }

    [Fact]
    public Task NestedTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            new CSharpClass("TestNested"),
        };

        return Verify(@class.ToString());
    }

    [Fact]
    public Task CommentTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            new CSharpMultiLineComment(writer => writer.WriteLine("Test")),
            new CSharpBlankLine(),
            new CSharpComment(writer => writer.WriteLine("Test")),
        };

        return Verify(@class.ToString());
    }

    [Fact]
    public Task AttributeTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            Attributes =
            {
                new CSharpAttribute("Test")
                {
                    new CSharpAttribute.Parameter("A"),
                    new CSharpAttribute.Parameter("b", "B"),
                    new CSharpAttribute.Property("C", "C"),
                },
            },
        };

        return Verify(@class.ToString());
    }

    [Fact]
    public Task FieldTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            new CSharpField(Visibility.Internal, "int", "Field")
            {
                IsReadonly = true,
                IsStatic = true,
                DefaultValue = "1",
            },
        };

        return Verify(@class.ToString());
    }

    [Fact]
    public Task MethodTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            new CSharpMethod(Visibility.Private, "int", "Method")
            {
                IsStatic = true,
                Body = writer => writer.WriteLine("return 1;"),
                Parameters =
                {
                    new CSharpParameter("string", "parameter")
                    {
                        Attributes =
                        {
                            new CSharpAttribute("Attribute1"),
                            new CSharpAttribute("Attribute2"),
                        },
                    },
                },
            },
        };

        return Verify(@class.ToString());
    }

    [Fact]
    public Task PropertyTest()
    {
        var @class = new CSharpClass("TestClass")
        {
            new CSharpProperty("int", "AutoProperty")
            {
                Getter = new CSharpProperty.Accessor(),
                Setter = new CSharpProperty.Accessor(),
            },
            new CSharpProperty("int", "GetOnlyExpressionProperty")
            {
                Getter = new CSharpProperty.Accessor
                {
                    Body = writer => writer.Write("1;"),
                },
            },
            new CSharpProperty("int", "ExpressionProperty")
            {
                Getter = new CSharpProperty.Accessor
                {
                    Body = writer => writer.Write("1;"),
                },
                Setter = new CSharpProperty.Accessor
                {
                    Body = writer => writer.Write("OnChanged(value);"),
                },
            },
            new CSharpProperty("int", "BlockProperty")
            {
                Getter = new CSharpProperty.Accessor
                {
                    BodyType = BodyType.Block,
                    Body = writer => writer.WriteLine("return 1;"),
                },
            },
        };

        return Verify(@class.ToString());
    }
}
