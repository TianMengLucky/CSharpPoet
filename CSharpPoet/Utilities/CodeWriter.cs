// ReSharper disable CheckNamespace

namespace CSharpPoet;

/// <summary>
///     IndentedTextWriter with additional utilities for writing C# source files.
/// </summary>
/// <inheritdoc />
public class CodeWriter(TextWriter writer, string tabString = IndentedTextWriter.DefaultTabString)
    : IndentedTextWriter(writer, tabString)
{
    private bool _isInMultilineComment;
    private bool _isInXmlComment;

    /// <inheritdoc />
    protected override void OutputTabs()
    {
        var tabsPending = TabsPending;
        base.OutputTabs();

        if (!tabsPending)
        {
            return;
        }

        if (_isInMultilineComment)
        {
            Write(" * ");
        }

        if (_isInXmlComment)
        {
            Write("/// ");
        }
    }

    /// <summary>
    ///     Writes a <see cref="Visibility" />.
    /// </summary>
    /// <param name="visibility">The visibility to write.</param>
    public void Write(Visibility visibility)
    {
        Write(visibility switch
        {
            Visibility.Public => "public",
            Visibility.Protected => "protected",
            Visibility.Internal => "internal",
            Visibility.ProtectedInternal => "protected internal",
            Visibility.Private => "private",
            Visibility.PrivateProtected => "private protected",
            Visibility.File => "file",
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        });
    }

    private static string? GetSeparator(ICSharpMember member)
    {
        return (member as IHasSeparator)?.Separator;
    }

    /// <summary>
    ///     Writes a list of members respecting their separators.
    /// </summary>
    /// <param name="members">Member list to be written.</param>
    /// <typeparam name="T">The type of members.</typeparam>
    public void WriteMembers<T>(IList<T> members) where T : ICSharpMember
    {
        if (members == null)
        {
            throw new ArgumentNullException(nameof(members));
        }

        for (var i = 0; i < members.Count; i++)
        {
            var member = members[i];
            var separator = GetSeparator(member);

            if (separator != null && i > 0)
            {
                Write(separator);
            }

            member.WriteTo(this);

            if (separator == null || i >= members.Count - 1)
            {
                continue;
            }

            var next = members[i + 1];
            if (GetSeparator(next) == null && next is not CSharpBlankLine)
            {
                Write(separator);
            }
        }
    }

    /// <summary>
    ///     Starts a multiline comment scope.
    /// </summary>
    /// <returns>Disposable <see cref="MultilineCommentScope" />.</returns>
    public MultilineCommentScope MultilineComment()
    {
        return new MultilineCommentScope(this);
    }

    /// <summary>
    ///     Starts a xml comment scope.
    /// </summary>
    /// <returns>Disposable <see cref="XmlCommentScope" />.</returns>
    public XmlCommentScope XmlComment()
    {
        return new XmlCommentScope(this);
    }

    /// <summary>
    ///     Starts an indent scope.
    /// </summary>
    /// <returns>Disposable <see cref="IndentScope" />.</returns>
    public IndentScope Indent()
    {
        return new IndentScope(this);
    }

    /// <summary>
    ///     Starts a block scope.
    /// </summary>
    /// <returns>Disposable <see cref="BlockScope" />.</returns>
    public BlockScope Block()
    {
        return new BlockScope(this);
    }

    /// <summary>
    ///     Sanitize an identifier so its usable in a C# source file.
    /// </summary>
    /// <param name="identifier">Identifier to be sanitized.</param>
    /// <returns>Sanitized identifier.</returns>
    public static string SanitizeIdentifier(string identifier)
    {
        return identifier switch
        {
            "abstract" or "as" or "base" or "bool" or "break" or "byte" or "case" or "catch" or "char" or "checked"
                or "class" or "const" or "continue" or "decimal" or "default" or "delegate" or "do" or "double"
                or "else" or "enum" or "event" or "explicit" or "extern" or "false" or "finally" or "fixed" or "float"
                or "for" or "foreach" or "goto" or "if" or "implicit" or "in" or "int" or "interface" or "internal"
                or "is" or "lock" or "long" or "namespace" or "new" or "null" or "object" or "operator" or "out"
                or "override" or "params" or "private" or "protected" or "public" or "readonly" or "ref" or "return"
                or "sbyte" or "sealed" or "short" or "sizeof" or "stackalloc" or "static" or "string" or "struct"
                or "switch" or "this" or "throw" or "true" or "try" or "typeof" or "uint" or "ulong" or "unchecked"
                or "unsafe" or "ushort" or "using" or "virtual" or "void" or "volatile" or "while"
                => "@" + identifier,
            _ => identifier
        };
    }

#pragma warning disable CA1815
    /// <summary>
    ///     Represents a block scope. Dispose to end it.
    /// </summary>
    /// <remarks>You can only have one at a time.</remarks>
    public readonly struct MultilineCommentScope : IDisposable
    {
        private readonly CodeWriter _writer;

        internal MultilineCommentScope(CodeWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));

            if (_writer._isInMultilineComment)
            {
                throw new ArgumentException("Can't enter a multiline comment twice");
            }

            _writer.WriteLine("/*");
            _writer._isInMultilineComment = true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _writer._isInMultilineComment = false;
            _writer.WriteLine(" */");
        }
    }

    /// <summary>
    ///     Represents a xml comment scope. Dispose to end it.
    /// </summary>
    /// <remarks>You can only have one at a time.</remarks>
    public readonly struct XmlCommentScope : IDisposable
    {
        private readonly CodeWriter _writer;

        internal XmlCommentScope(CodeWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));

            if (_writer._isInXmlComment)
            {
                throw new ArgumentException("Can't enter a xml comment twice");
            }

            _writer._isInXmlComment = true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _writer._isInXmlComment = false;
        }
    }

    /// <summary>
    ///     Represents an indent scope. Dispose to end it.
    /// </summary>
    public readonly struct IndentScope : IDisposable
    {
        private readonly CodeWriter _writer;

        internal IndentScope(CodeWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));

            _writer.IndentLevel++;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _writer.IndentLevel--;
        }
    }

    /// <summary>
    ///     Represents a block scope. Dispose to end it.
    /// </summary>
    public readonly struct BlockScope : IDisposable
    {
        private readonly CodeWriter _writer;

        internal BlockScope(CodeWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));

            _writer.WriteLine('{');
            _writer.IndentLevel++;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _writer.IndentLevel--;
            _writer.WriteLine('}');
        }
    }
#pragma warning restore CA1815
}
