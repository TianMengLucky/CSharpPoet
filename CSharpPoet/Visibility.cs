namespace CSharpPoet;

// https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/accessibility-levels
/// <summary>
/// Visibility of a C# member.
/// </summary>
public enum Visibility
{
    /**
     * Access is not restricted.
     */
    Public,

    /**
     * Access is limited to the containing class or types derived from the containing class.
     */
    Protected,

    /**
     * Access is limited to the current assembly.
     */
    Internal,

    /**
     * Access is limited to the current assembly or types derived from the containing class.
     */
    ProtectedInternal,

    /**
     * Access is limited to the containing type.
     */
    Private,

    /**
     * Access is limited to the containing class or types derived from the containing class within the current assembly. Available since C# 7.2.
     */
    PrivateProtected,

    /**
     * The declared type is only visible in the current source file. File scoped types are generally used for source generators.
     */
    File,
}
