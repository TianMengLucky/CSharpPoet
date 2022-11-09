namespace CSharpPoet.Traits;

public interface IHasModifiers
{
    Modifiers Modifiers { get; set; }
}

internal static class IHasModifiersExtensions
{
    private static readonly Dictionary<Modifiers, string> _names = new()
    {
        [Modifiers.Static] = "static",
        [Modifiers.Extern] = "extern",
        [Modifiers.New] = "new",
        [Modifiers.Virtual] = "virtual",
        [Modifiers.Abstract] = "abstract",
        [Modifiers.Sealed] = "sealed",
        [Modifiers.Override] = "override",
        [Modifiers.Readonly] = "readonly",
        [Modifiers.Unsafe] = "unsafe",
        [Modifiers.Required] = "required",
        [Modifiers.Volatile] = "volatile",
        [Modifiers.Async] = "async",
        [Modifiers.Partial] = "partial",
        [Modifiers.Const] = "const",
    };

    public static void WriteModifiersTo(this IHasModifiers self, CodeWriter writer)
    {
        foreach (var kv in _names)
        {
            if (self.HasModifier(kv.Key))
            {
                writer.Write(kv.Value);
                writer.Write(' ');
            }
        }
    }

    public static bool HasModifier(this IHasModifiers self, Modifiers modifier)
    {
        return (self.Modifiers & modifier) == modifier;
    }

    public static void SetModifier(this IHasModifiers self, Modifiers modifier, bool value)
    {
        if (value)
        {
            self.Modifiers |= modifier;
        }
        else
        {
            self.Modifiers &= ~modifier;
        }
    }
}
