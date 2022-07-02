namespace CSharpPoet;

public class CSharpProperty : CSharpType.IMember, IHasSeparator
{
    string? IHasSeparator.Separator => IsMultiline ? "\n" : null;

    public Visibility Visibility { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }

    public bool IsStatic { get; set; }

    public Accessor? Getter { get; set; }
    public Accessor? Setter { get; set; }

    public bool IsInitOnly { get; set; }

    public string? DefaultValue { get; set; }

    public bool IsMultiline => Getter is { IsMultiline: true } || Setter is { IsMultiline: true };

    public CSharpProperty(Visibility visibility, string type, string name)
    {
        Visibility = visibility;
        Type = type;
        Name = name;
    }

    public CSharpProperty(string type, string name) : this(Visibility.Public, type, name)
    {
    }

    /// <inheritdoc />
    public void WriteTo(CodeWriter writer)
    {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        writer.Write(Visibility);
        writer.Write(' ');

        if (IsStatic) writer.Write("static ");

        writer.Write(Type);
        writer.Write(' ');

        writer.Write(CodeWriter.SanitizeIdentifier(Name));

        writer.Write(' ');

        if (Getter is { Body: { }, IsMultiline: false } && Setter == null)
        {
            writer.Write("=> ");
            Getter.Body(writer);
            writer.WriteLine();
        }
        else
        {
            CodeWriter.BlockScope? blockScope = null;

            if (IsMultiline)
            {
                writer.WriteLine();
                blockScope = writer.Block();
            }
            else
            {
                writer.Write('{');
            }

            if (Getter == null && Setter == null)
            {
                writer.Write(' ');
            }
            else
            {
                if (Getter != null)
                {
                    if (blockScope == null) writer.Write(" ");
                    Getter.WriteTo(writer, "get");

                    if (blockScope == null && Setter == null) writer.Write(" ");
                    else if (blockScope != null && Setter != null) writer.WriteLine();
                }

                if (Setter != null)
                {
                    if (blockScope == null) writer.Write(" ");
                    Setter.WriteTo(writer, IsInitOnly ? "init" : "set");
                    if (blockScope == null) writer.Write(" ");
                }
            }

            if (blockScope != null)
            {
                blockScope.Value.Dispose();
            }
            else
            {
                writer.Write('}');

                if (DefaultValue != null)
                {
                    writer.Write(" = ");
                    writer.Write(DefaultValue);
                    writer.Write(';');
                }

                writer.WriteLine();
            }
        }
    }

    public class Accessor
    {
        public Visibility? Visibility { get; set; }

        public bool IsMultiline { get; set; }

        public Action<CodeWriter>? Body { get; set; }

        public Accessor()
        {
        }

        public Accessor(Visibility? visibility)
        {
            Visibility = visibility;
        }

        public void WriteTo(CodeWriter writer, string name)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            if (Visibility != null)
            {
                writer.Write(Visibility.Value);
                writer.Write(' ');
            }

            writer.Write(name);

            if (Body != null)
            {
                if (IsMultiline)
                {
                    writer.WriteLine();
                    using (writer.Block())
                    {
                        Body(writer);
                    }
                }
                else
                {
                    writer.Write(" => ");
                    Body(writer);
                }
            }
            else
            {
                writer.Write(';');
            }
        }
    }
}
