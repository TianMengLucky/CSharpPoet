namespace CSharpPoet;

[Flags]
public enum Modifiers
{
    None = 1 << 0,
    Static = 1 << 1,
    Extern = 1 << 2,
    New = 1 << 3,
    Virtual = 1 << 4,
    Abstract = 1 << 5,
    Sealed = 1 << 6,
    Override = 1 << 7,
    Readonly = 1 << 8,
    Unsafe = 1 << 9,
    Required = 1 << 10,
    Volatile = 1 << 11,
    Async = 1 << 12,
    Partial = 1 << 13,
    Const = 1 << 14,
}
