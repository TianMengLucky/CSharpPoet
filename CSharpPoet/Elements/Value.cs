namespace CSharpPoet;

public class Value(string value)
{
    public string _Value { get; set; } = value;

    public virtual void Write(CodeWriter writer)
    {
        writer.Write(" = ");
        writer.Write(_Value);
        if (!_Value.EndsWith(";"))
        {
            writer.Write(";");
        }
    }
}

public class FiledValue(CSharpField field) : Value(field.Name);

public class StringValue(string text) : Value($"\"{text}\"")
{
    public string text { get; set; } = text;

    public static implicit operator string(StringValue value)
    {
        return value.text;
    }

    public static implicit operator StringValue(string value)
    {
        return new StringValue(value);
    }
}

public class NumberValue<T>(T value) : Value(value.ToString()) where T : struct, IFormattable;

public class NewValue(string className, params string[] parameters) : Value("new ")
{
    public string ClassName { get; set; } = className;
    public bool HasTypeName { get; set; } = true;

    public string[] Parameters { get; set; } = parameters;

    public override void Write(CodeWriter writer)
    {
        if (HasTypeName)
        {
            _Value += ClassName;
        }

        _Value += "(";
        _Value += ")";
        base.Write(writer);
    }
}
