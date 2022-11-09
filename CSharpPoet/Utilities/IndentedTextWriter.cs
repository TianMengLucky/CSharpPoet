// Taken from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/CodeDom/Compiler/IndentedTextWriter.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace CSharpPoet;

/// <summary>
/// TextWriter with automatic indentation. 
/// </summary>
public class IndentedTextWriter : TextWriter
{
    private readonly TextWriter _writer;
    private readonly string _tabString;
    private int _indentLevelLevel;
    private bool _tabsPending;

    /// <summary>
    /// Gets default tab string.
    /// </summary>
    public const string DefaultTabString = "    ";

    /// <inheritdoc/>
    public IndentedTextWriter(TextWriter writer, string tabString = DefaultTabString) : base(CultureInfo.InvariantCulture)
    {
        _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        _tabString = tabString;
    }

    /// <summary>
    /// Gets encoding.
    /// </summary>
    public override Encoding Encoding => _writer.Encoding;

    /// <summary>
    /// Gets or sets new line.
    /// </summary>
    [AllowNull]
    public override string NewLine
    {
        get => _writer.NewLine;
        set => _writer.NewLine = value;
    }

    /// <summary>
    /// Gets or sets the indentation level.
    /// </summary>
    public int IndentLevel
    {
        get => _indentLevelLevel;
        set => _indentLevelLevel = Math.Max(value, 0);
    }

    /// <summary>
    /// Gets the value indicating whether indentation should be written out.
    /// </summary>
    protected bool TabsPending => _tabsPending;

    /// <summary>
    /// Gets the inner TextWriter.
    /// </summary>
    public TextWriter InnerWriter => _writer;

    /// <inheritdoc/>
    public override void Close() => _writer.Close();

#pragma warning disable CA1816
#pragma warning disable CA2215
    /// <inheritdoc/>
    protected override void Dispose(bool disposing) => _writer.Dispose();

#if NET
    /// <inheritdoc/>
    public override ValueTask DisposeAsync() => _writer.DisposeAsync();
#endif
#pragma warning restore CA1816
#pragma warning restore CA2215

    /// <inheritdoc/>
    public override void Flush() => _writer.Flush();

    /// <inheritdoc/>
    public override Task FlushAsync() => _writer.FlushAsync();

    /// <summary>
    /// Outputs tabs to the underlying <see cref="TextWriter"/> based on the current <see cref="IndentLevel"/>.
    /// </summary>
    protected virtual void OutputTabs()
    {
        if (_tabsPending)
        {
            for (var i = 0; i < _indentLevelLevel; i++)
            {
                _writer.Write(_tabString);
            }

            _tabsPending = false;
        }
    }

    /// <summary>
    /// Asynchronously outputs tabs to the underlying <see cref="TextWriter"/> based on the current <see cref="IndentLevel"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual async Task OutputTabsAsync()
    {
        if (_tabsPending)
        {
            for (var i = 0; i < _indentLevelLevel; i++)
            {
                await _writer.WriteAsync(_tabString).ConfigureAwait(false);
            }

            _tabsPending = false;
        }
    }

    /// <inheritdoc/>
    public override void Write(string? value)
    {
        if (value == "\n")
        {
            WriteLine();
            return;
        }

        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(bool value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(char value)
    {
        if (value == '\n')
        {
            WriteLine();
            return;
        }

        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(char[]? buffer)
    {
        OutputTabs();
        _writer.Write(buffer);
    }

    /// <inheritdoc/>
    public override void Write(char[] buffer, int index, int count)
    {
        OutputTabs();
        _writer.Write(buffer, index, count);
    }

    /// <inheritdoc/>
    public override void Write(double value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(float value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(int value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(long value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(object? value)
    {
        OutputTabs();
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(string format, object? arg0)
    {
        OutputTabs();
        _writer.Write(format, arg0);
    }

    /// <inheritdoc/>
    public override void Write(string format, object? arg0, object? arg1)
    {
        OutputTabs();
        _writer.Write(format, arg0, arg1);
    }

    /// <inheritdoc/>
    public override void Write(string format, params object?[] arg)
    {
        OutputTabs();
        _writer.Write(format, arg);
    }

    /// <summary>
    /// Asynchronously writes the specified <see cref="char"/> to the underlying <see cref="TextWriter"/>, inserting
    /// tabs at the start of every line.
    /// </summary>
    /// <param name="value">The <see cref="char"/> to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(char value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteAsync(value).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously writes the specified number of <see cref="char"/>s from the specified buffer
    /// to the underlying <see cref="TextWriter"/>, starting at the specified index, and outputting tabs at the
    /// start of every new line.
    /// </summary>
    /// <param name="buffer">The array to write from.</param>
    /// <param name="index">Index in the array to stort writing at.</param>
    /// <param name="count">The number of characters to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(char[] buffer, int index, int count)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteAsync(buffer, index, count).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously writes the specified string to the underlying <see cref="TextWriter"/>, inserting tabs at the
    /// start of every line.
    /// </summary>
    /// <param name="value">The string to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(string? value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteAsync(value).ConfigureAwait(false);
    }

#if NET
    /// <summary>
    /// Asynchronously writes the specified characters to the underlying <see cref="TextWriter"/>, inserting tabs at the
    /// start of every line.
    /// </summary>
    /// <param name="buffer">The characters to write.</param>
    /// <param name="cancellationToken">Token for canceling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously writes the contents of the specified <see cref="StringBuilder"/> to the underlying <see cref="TextWriter"/>, inserting tabs at the
    /// start of every line.
    /// </summary>
    /// <param name="value">The text to write.</param>
    /// <param name="cancellationToken">Token for canceling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteAsync(value, cancellationToken).ConfigureAwait(false);
    }
#endif

    /// <summary>
    /// Writes a string followed by a line terminator to the text string or stream without indentation.
    /// </summary>
    /// <param name="value">The string to write. If value is null, only the line terminator is written.</param>
    public void WriteLineNoTabs(string? value)
    {
        _writer.WriteLine(value);
    }

    /// <summary>
    /// Asynchronously writes the specified string to the underlying <see cref="TextWriter"/> without inserting tabs.
    /// </summary>
    /// <param name="s">The string to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task WriteLineNoTabsAsync(string? s)
    {
        return _writer.WriteLineAsync(s);
    }

    /// <inheritdoc />
    public override void WriteLine(string? value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine()
    {
        _writer.WriteLine();
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(bool value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(char value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(char[]? buffer)
    {
        OutputTabs();
        _writer.WriteLine(buffer);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(char[] buffer, int index, int count)
    {
        OutputTabs();
        _writer.WriteLine(buffer, index, count);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(double value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(float value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(int value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(long value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(object? value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(string format, object? arg0)
    {
        OutputTabs();
        _writer.WriteLine(format, arg0);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(string format, object? arg0, object? arg1)
    {
        OutputTabs();
        _writer.WriteLine(format, arg0, arg1);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(string format, params object?[] arg)
    {
        OutputTabs();
        _writer.WriteLine(format, arg);
        _tabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(uint value)
    {
        OutputTabs();
        _writer.WriteLine(value);
        _tabsPending = true;
    }

    /// <inheritdoc/>
    public override async Task WriteLineAsync()
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteLineAsync().ConfigureAwait(false);
        _tabsPending = true;
    }

    /// <summary>
    /// Asynchronously writes the specified <see cref="char"/> to the underlying <see cref="TextWriter"/> followed by a line terminator, inserting tabs
    /// at the start of every line.
    /// </summary>
    /// <param name="value">The character to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(char value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteLineAsync(value).ConfigureAwait(false);
        _tabsPending = true;
    }

    /// <summary>
    /// Asynchronously writes the specified number of characters from the specified buffer followed by a line terminator,
    /// to the underlying <see cref="TextWriter"/>, starting at the specified index within the buffer, inserting tabs at the start of every line.
    /// </summary>
    /// <param name="buffer">The buffer containing characters to write.</param>
    /// <param name="index">The index within the buffer to start writing at.</param>
    /// <param name="count">The number of characters to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(char[] buffer, int index, int count)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteLineAsync(buffer, index, count).ConfigureAwait(false);
        _tabsPending = true;
    }

    /// <summary>
    /// Asynchronously writes the specified string followed by a line terminator to the underlying <see cref="TextWriter"/>, inserting
    /// tabs at the start of every line.
    /// </summary>
    /// <param name="value">The string to write.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(string? value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteLineAsync(value).ConfigureAwait(false);
        _tabsPending = true;
    }

#if NET
    /// <summary>
    /// Asynchronously writes the specified characters followed by a line terminator to the underlying <see cref="TextWriter"/>, inserting
    /// tabs at the start of every line.
    /// </summary>
    /// <param name="buffer">The characters to write.</param>
    /// <param name="cancellationToken">Token for canceling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteLineAsync(buffer, cancellationToken).ConfigureAwait(false);
        _tabsPending = true;
    }

    /// <summary>
    /// Asynchronously writes the contents of the specified <see cref="StringBuilder"/> followed by a line terminator to the
    /// underlying <see cref="TextWriter"/>, inserting tabs at the start of every line.
    /// </summary>
    /// <param name="value">The text to write.</param>
    /// <param name="cancellationToken">Token for canceling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await _writer.WriteLineAsync(value, cancellationToken).ConfigureAwait(false);
        _tabsPending = true;
    }
#endif
}
