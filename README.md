# CSharpPoet

[![NuGet](https://img.shields.io/nuget/v/CSharpPoet?logo=NuGet)](https://www.nuget.org/packages/CSharpPoet)

A C# library for generating C# source files.

## Example

```csharp
new CSharpFile("TestNamespace")
{
    new CSharpClass("TestClass")
    {
        new CSharpField("int", "TestField")
        {
            IsStatic = true,
            IsReadonly = true,
            DefaultValue = "1",
        },
    },
}.WriteTo(Console.Out);
```

generates

```csharp
namespace TestNamespace;

public class TestClass
{
    public static readonly int TestField = 1;
}
```
