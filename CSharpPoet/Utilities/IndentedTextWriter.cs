// Taken from https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/CodeDom/Compiler/IndentedTextWriter.cs

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

// ReSharper disable CheckNamespace

namespace CSharpPoet;

/// <summary>
///     TextWriter with automatic indentation.
/// </summary>
/// <inheritdoc />
public class IndentedTextWriter(TextWriter writer, string tabString = IndentedTextWriter.DefaultTabString)
    : TextWriter(CultureInfo.InvariantCulture)
{
    private int _indentLevelLevel;

    /// <summary>
    ///     Gets default tab string.
    /// </summary>
    public const string DefaultTabString = "    ";

    /// <summary>
    ///     Gets encoding.
    /// </summary>
    public override Encoding Encoding => InnerWriter.Encoding;

    /// <summary>
    ///     Gets or sets new line.
    /// </summary>
    [AllowNull]
    public override string NewLine
    {
        get => InnerWriter.NewLine;
        set => InnerWriter.NewLine = value;
    }

    /// <summary>
    ///     Gets or sets the indentation level.
    /// </summary>
    public int IndentLevel
    {
        get => _indentLevelLevel;
        set => _indentLevelLevel = Math.Max(value, 0);
    }

    /// <summary>
    ///     Gets the value indicating whether indentation should be written out.
    /// </summary>
    protected bool TabsPending { get; private set; }

    /// <summary>
    ///     Gets the inner TextWriter.
    /// </summary>
    public TextWriter InnerWriter { get; } = writer ?? throw new ArgumentNullException(nameof(writer));

    /// <inheritdoc />
    public override void Close()
    {
        InnerWriter.Close();
    }

#pragma warning disable CA1816
#pragma warning disable CA2215
    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        InnerWriter.Dispose();
    }

#if NET
    /// <inheritdoc/>
    public override ValueTask DisposeAsync() => InnerWriter.DisposeAsync();
#endif
#pragma warning restore CA1816
#pragma warning restore CA2215

    /// <inheritdoc />
    public override void Flush()
    {
        InnerWriter.Flush();
    }

    /// <inheritdoc />
    public override Task FlushAsync()
    {
        return InnerWriter.FlushAsync();
    }

    /// <summary>
    ///     Outputs tabs to the underlying <see cref="TextWriter" /> based on the current <see cref="IndentLevel" />.
    /// </summary>
    protected virtual void OutputTabs()
    {
        if (TabsPending)
        {
            for (var i = 0; i < _indentLevelLevel; i++)
            {
                InnerWriter.Write(tabString);
            }

            TabsPending = false;
        }
    }

    /// <summary>
    ///     Asynchronously outputs tabs to the underlying <see cref="TextWriter" /> based on the current
    ///     <see cref="IndentLevel" />.
    /// </summary>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    protected virtual async Task OutputTabsAsync()
    {
        if (TabsPending)
        {
            for (var i = 0; i < _indentLevelLevel; i++)
            {
                await InnerWriter.WriteAsync(tabString).ConfigureAwait(false);
            }

            TabsPending = false;
        }
    }

    /// <inheritdoc />
    public override void Write(string? value)
    {
        if (value == "\n")
        {
            WriteLine();
            return;
        }

        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(bool value)
    {
        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(char value)
    {
        if (value == '\n')
        {
            WriteLine();
            return;
        }

        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(char[]? buffer)
    {
        OutputTabs();
        InnerWriter.Write(buffer);
    }

    /// <inheritdoc />
    public override void Write(char[] buffer, int index, int count)
    {
        OutputTabs();
        InnerWriter.Write(buffer, index, count);
    }

    /// <inheritdoc />
    public override void Write(double value)
    {
        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(float value)
    {
        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(int value)
    {
        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(long value)
    {
        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(object? value)
    {
        OutputTabs();
        InnerWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(string format, object? arg0)
    {
        OutputTabs();
        InnerWriter.Write(format, arg0);
    }

    /// <inheritdoc />
    public override void Write(string format, object? arg0, object? arg1)
    {
        OutputTabs();
        InnerWriter.Write(format, arg0, arg1);
    }

    /// <inheritdoc />
    public override void Write(string format, params object?[] arg)
    {
        OutputTabs();
        InnerWriter.Write(format, arg);
    }

    /// <summary>
    ///     Asynchronously writes the specified <see cref="char" /> to the underlying <see cref="TextWriter" />, inserting
    ///     tabs at the start of every line.
    /// </summary>
    /// <param name="value">The <see cref="char" /> to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(char value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteAsync(value).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously writes the specified number of <see cref="char" />s from the specified buffer
    ///     to the underlying <see cref="TextWriter" />, starting at the specified index, and outputting tabs at the
    ///     start of every new line.
    /// </summary>
    /// <param name="buffer">The array to write from.</param>
    /// <param name="index">Index in the array to stort writing at.</param>
    /// <param name="count">The number of characters to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(char[] buffer, int index, int count)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteAsync(buffer, index, count).ConfigureAwait(false);
    }

    /// <summary>
    ///     Asynchronously writes the specified string to the underlying <see cref="TextWriter" />, inserting tabs at the
    ///     start of every line.
    /// </summary>
    /// <param name="value">The string to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public override async Task WriteAsync(string? value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteAsync(value).ConfigureAwait(false);
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
        await InnerWriter.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
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
        await InnerWriter.WriteAsync(value, cancellationToken).ConfigureAwait(false);
    }
#endif

    /// <summary>
    ///     Writes a string followed by a line terminator to the text string or stream without indentation.
    /// </summary>
    /// <param name="value">The string to write. If value is null, only the line terminator is written.</param>
    public void WriteLineNoTabs(string? value)
    {
        InnerWriter.WriteLine(value);
    }

    /// <summary>
    ///     Asynchronously writes the specified string to the underlying <see cref="TextWriter" /> without inserting tabs.
    /// </summary>
    /// <param name="s">The string to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public Task WriteLineNoTabsAsync(string? s)
    {
        return InnerWriter.WriteLineAsync(s);
    }

    /// <inheritdoc />
    public override void WriteLine(string? value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine()
    {
        InnerWriter.WriteLine();
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(bool value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(char value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(char[]? buffer)
    {
        OutputTabs();
        InnerWriter.WriteLine(buffer);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(char[] buffer, int index, int count)
    {
        OutputTabs();
        InnerWriter.WriteLine(buffer, index, count);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(double value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(float value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(int value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(long value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(object? value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(string format, object? arg0)
    {
        OutputTabs();
        InnerWriter.WriteLine(format, arg0);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(string format, object? arg0, object? arg1)
    {
        OutputTabs();
        InnerWriter.WriteLine(format, arg0, arg1);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(string format, params object?[] arg)
    {
        OutputTabs();
        InnerWriter.WriteLine(format, arg);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine(uint value)
    {
        OutputTabs();
        InnerWriter.WriteLine(value);
        TabsPending = true;
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync()
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteLineAsync().ConfigureAwait(false);
        TabsPending = true;
    }

    /// <summary>
    ///     Asynchronously writes the specified <see cref="char" /> to the underlying <see cref="TextWriter" /> followed by a
    ///     line terminator, inserting tabs
    ///     at the start of every line.
    /// </summary>
    /// <param name="value">The character to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(char value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteLineAsync(value).ConfigureAwait(false);
        TabsPending = true;
    }

    /// <summary>
    ///     Asynchronously writes the specified number of characters from the specified buffer followed by a line terminator,
    ///     to the underlying <see cref="TextWriter" />, starting at the specified index within the buffer, inserting tabs at
    ///     the start of every line.
    /// </summary>
    /// <param name="buffer">The buffer containing characters to write.</param>
    /// <param name="index">The index within the buffer to start writing at.</param>
    /// <param name="count">The number of characters to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(char[] buffer, int index, int count)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteLineAsync(buffer, index, count).ConfigureAwait(false);
        TabsPending = true;
    }

    /// <summary>
    ///     Asynchronously writes the specified string followed by a line terminator to the underlying
    ///     <see cref="TextWriter" />, inserting
    ///     tabs at the start of every line.
    /// </summary>
    /// <param name="value">The string to write.</param>
    /// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(string? value)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteLineAsync(value).ConfigureAwait(false);
        TabsPending = true;
    }

#if NET
    /// <summary>
    /// Asynchronously writes the specified characters followed by a line terminator to the underlying <see cref="TextWriter"/>, inserting
    /// tabs at the start of every line.
    /// </summary>
    /// <param name="buffer">The characters to write.</param>
    /// <param name="cancellationToken">Token for canceling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public override async Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken =
 default)
    {
        await OutputTabsAsync().ConfigureAwait(false);
        await InnerWriter.WriteLineAsync(buffer, cancellationToken).ConfigureAwait(false);
        TabsPending = true;
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
        await InnerWriter.WriteLineAsync(value, cancellationToken).ConfigureAwait(false);
        TabsPending = true;
    }
#endif
}
