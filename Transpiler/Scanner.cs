namespace Transpiler;

/// <summary>
/// Scans a raw code source to a list of valid tokens. Also resolves primitive types.
/// </summary>
/// <param name="source">Raw source code to be scanned into tokens.</param>
public class Scanner(string source)
{
    private int _start;
    private int _current;
    private uint _line = 1;

    private readonly List<Token> _tokens = [];

    /// <summary>
    /// Begin scanning the source and generate a list of valid tokens.
    /// </summary>
    /// <returns>List of valid tokens corresponding to the source.</returns>
    public List<Token> ScanTokens()
    {
        while (!AtEnd())
        {
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.Eof, "", null, _line));
        return _tokens;
    }

    /// <summary>
    /// Scan the current word of the source and generate its corresponding token.
    /// </summary>
    private void ScanToken()
    {
        char character = Advance();

        switch (character)
        {
            case '(':
                AddToken(TokenType.LeftParenthesis);
                break;

            case ')':
                AddToken(TokenType.RightParenthesis);
                break;

            case '{':
                AddToken(TokenType.LeftBrace);
                break;

            case '}':
                AddToken(TokenType.RightBrace);
                break;

            case '[':
                AddToken(TokenType.LeftBracket);
                break;

            case ']':
                AddToken(TokenType.RightBracket);
                break;

            case ',':
                AddToken(TokenType.Comma);
                break;

            case '.':
                AddToken(TokenType.Dot);
                break;

            case ':':
                AddToken(TokenType.Colon);
                break;

            case '+':
                AddToken(TokenType.Plus);
                break;

            case '-':
                AddToken(TokenType.Minus);
                break;

            case '*':
                AddToken(TokenType.Asterisk);
                break;

            case '/':
                AddToken(TokenType.Slash);
                break;

            case '%':
                AddToken(TokenType.Percent);
                break;

            case '!':
                AddToken(Match() ? TokenType.BangEquals : TokenType.Bang);
                break;

            case '=':
                AddToken(Match() ? TokenType.EqualEquals : TokenType.Equal);
                break;

            case '>':
                AddToken(Match() ? TokenType.GreaterThanEquals : TokenType.GreaterThan);
                break;

            case '<':
                AddToken(Match() ? TokenType.LesserThanEquals : TokenType.LesserThan);
                break;

            case ' ' or '\r' or '\t':
                break;

            case '\n':
                _line++;
                break;

            default:
                if (char.IsLetterOrDigit(character) || character == '_')
                    ScanIdentifier();

                else
                    Dbir.Error(_line, $"Unexpected character: {character}.");
                break;
        }
    }

    /// <summary>
    /// Scan an identifier that does not directly correspond to raw tokens.
    /// </summary>
    private void ScanIdentifier()
    {
        while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
            Advance();

        string text = source.Substring(_start, _current - _start);
        if (Token.IsDataType(text))
            ResolveDataType(text);

        else
            AddToken(Token.LookupIdentifier(text));
    }

    /// <summary>
    /// Resolve a source substring that corresponds to a data type.
    /// </summary>
    /// <param name="text">Substring that corresponds to a data type.</param>
    private void ResolveDataType(string text)
    {
        TokenType dataType = Token.LookupIdentifier(text);

        DataType? type = null;
        switch (dataType)
        {
            case TokenType.Int0 or TokenType.Int8 or TokenType.Int16 or TokenType.Int24 or TokenType.Int32
                or TokenType.Int48 or TokenType.Int64:
                type = ResolveInt(dataType);
                break;

            case TokenType.UInt0 or TokenType.UInt8 or TokenType.UInt16 or TokenType.UInt24 or TokenType.UInt32
                or TokenType.UInt48 or TokenType.UInt64:
                type = ResolveUInt(dataType);
                break;

            case TokenType.Float32 or TokenType.Float64:
                type = ResolveFloat(dataType);
                break;

            case TokenType.Decimal:
                type = ResolveDecimal();
                break;

            case TokenType.Boolean:
                type = ResolveBoolean();
                break;

            case TokenType.BitField or TokenType.ByteField or TokenType.CharField:
                type = ResolveField(dataType);
                break;

            case TokenType.Date or TokenType.Time or TokenType.DateTime or TokenType.Interval:
                type = ResolveDates(dataType);
                break;

            case TokenType.Json:
                type = ResolveJson();
                break;

            case TokenType.Pointer:
                type = ResolvePointer();
                break;

            case TokenType.Option or TokenType.Some:
                type = ResolveOption();
                break;

            case TokenType.None:
                type = new NoneType();
                break;

            default:
                Dbir.Error(_line, $"Invalid data-type {dataType} encountered.");
                break;
        }

        if (type is not null)
            AddToken(dataType, type);
    }

    /// <summary>
    /// Try to resolve an integer type. Logs a DBIR error if it fails.
    /// </summary>
    /// <param name="intType">Type of integer to resolve; corresponds to its storage class.</param>
    /// <returns>Resolved integer type if successful else <c>null</c>.</returns>
    private IntType? ResolveInt(TokenType intType)
    {
        IntStorage storage = IntType.GetStorageFromTokenType(intType);

        if (Peek() != '[')
            return new IntType(storage);

        Advance();
        var number = string.Empty;
        while (Peek() != ']')
            number += Advance();
        Advance();

        if (long.TryParse(number, out long value))
            return new IntType(storage, value);

        Dbir.Error(_line, $"Unable to parse value {number} as an integer.");
        return null;
    }

    /// <summary>
    /// Try to resolve an unsigned integer type. Logs a DBIR error if it fails.
    /// </summary>
    /// <param name="uintType">Type of unsigned integer to resolve; corresponds to its storage class.</param>
    /// <returns>Resolved unsigned integer type if successful else <c>null</c>.</returns>
    private UIntType? ResolveUInt(TokenType uintType)
    {
        UIntStorage storage = UIntType.GetStorageFromTokenType(uintType);

        if (Peek() != '[')
            return new UIntType(storage);

        Advance();
        var number = string.Empty;
        while (Peek() != ']')
            number += Advance();
        Advance();

        if (ulong.TryParse(number, out ulong value))
            return new UIntType(storage, value);

        Dbir.Error(_line, $"Unable to parse value {number} as an unsigned long integer.");
        return null;
    }

    /// <summary>
    /// Try to resolve a floating-point number type. Logs a DBIR error if it fails.
    /// </summary>
    /// <param name="floatType">Type of floating-point number to resolve; corresponds to its storage class.</param>
    /// <returns>Resolved floating-point number type if successful else <c>null</c>.</returns>
    private FloatType? ResolveFloat(TokenType floatType)
    {
        FloatStorage storage = FloatType.GetStorageFromTokenType(floatType);

        if (Peek() != '[')
            return new FloatType(storage);

        Advance();
        var number = string.Empty;
        while (Peek() != ']')
            number += Advance();
        Advance();

        if (double.TryParse(number, out double value))
            return new FloatType(storage, value);

        Dbir.Error(_line, $"Unable to parse {number} as a double.");
        return null;
    }
    
    /// <summary>
    /// Try to resolve a decimal number type. Logs a DBIR error if it fails. 
    /// </summary>
    /// <returns>Resolved decimal number type if successful else <c>null</c>.</returns>
    private DecimalType? ResolveDecimal()
    {
        if (Peek() != '[')
            return new DecimalType();

        Advance();
        var number = string.Empty;
        while (Peek() != ',')
            number += Advance();
        Advance();

        if (!decimal.TryParse(number, out decimal value))
        {
            Dbir.Error(_line, $"Unable to parse {number} as a decimal.");
            return null;
        }

        uint? precision = ParseUInt(',');
        if (precision is null)
            return null;

        uint? scale = ParseUInt(']');
        if (scale is null)
            return null;

        return new DecimalType(value, precision, scale);
    }

    /// <summary>
    /// Try to resolve a boolean type. Logs a DBIR error if it fails. 
    /// </summary>
    /// <returns>Resolved boolean type if successful else <c>null</c>.</returns>
    private BooleanType? ResolveBoolean()
    {
        if (Peek() != '[')
            return new BooleanType();

        Advance();
        var data = string.Empty;
        while (Peek() != ']')
            data += Advance();
        Advance();

        switch (data)
        {
            case "true":
                return new BooleanType(true);

            case "false":
                return new BooleanType(false);

            default:
                Dbir.Error(_line, $"Unable to parse {data} as a boolean.");
                return null;
        }
    }

    /// <summary>
    /// Tru to resolve a field type. Logs a DBIR error if it fails.
    /// </summary>
    /// <param name="fieldType">Type of field to resolve, could be a bit-field, byte-field or char-field.</param>
    /// <returns>Resolved field type if successful else <c>null</c>.</returns>
    private DataType? ResolveField(TokenType fieldType)
    {
        Type field = fieldType switch
        {
            TokenType.BitField => typeof(BitFieldType),
            TokenType.ByteField => typeof(ByteFieldType),
            _ => typeof(CharFieldType)
        };

        if (Peek() != '[')
            return (DataType?)Activator.CreateInstance(field, [null, false]);

        Advance();
        var rawString = string.Empty;
        while (Peek() != ']')
            rawString += Advance();
        Advance();

        Advance();
        var resizableString = string.Empty;
        while (Peek() != ']')
            resizableString += Advance();
        Advance();

        if (!bool.TryParse(resizableString, out bool resizable))
        {
            Dbir.Error(_line, $"Unable to parse {resizableString} as a boolean.");
            return null;
        }

        string[] rawArray = rawString.Trim('[', ']').Split(',');
        switch (fieldType)
        {
            case TokenType.BitField:
            {
                bool[] bits = rawArray.Select(bool.Parse).ToArray();
                return new BitFieldType(bits, resizable);
            }

            case TokenType.ByteField:
            {
                byte[] bytes = rawArray.Select(byte.Parse).ToArray();
                return new ByteFieldType(bytes, resizable);
            }

            default:
            {
                char[] chars = rawArray.Select(s => s.Trim()[1]).ToArray();
                return new CharFieldType(chars, resizable);
            }
        }
    }

    /// <summary>
    /// Try to resolve a generic date type.
    /// </summary>
    /// <param name="dateType">Type of generic date to resolve, could be a date, time, date-time or interval.</param>
    /// <returns>Resolved generic date type if successful else <c>null</c>.</returns>
    private DataType? ResolveDates(TokenType dateType)
    {
        return dateType switch
        {
            TokenType.Date => ResolveDate(),
            TokenType.Time => ResolveTime(),
            TokenType.DateTime => ResolveDateTime(),
            _ => ResolveInterval()
        };
    }

    /// <summary>
    /// Try to resolve a date type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved date type if successful else <c>null</c>.</returns>
    private DateType? ResolveDate()
    {
        if (Peek() != '[')
            return new DateType();

        Advance();
        uint? date = ParseUInt(',');
        if (date is null)
            return null;

        uint? month = ParseUInt(',');
        if (month is null)
            return null;

        uint? year = ParseUInt(']');
        if (year is null)
            return null;

        return new DateType(date, month, year);
    }

    /// <summary>
    /// Try to resolve a time type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved time type if successful else <c>null</c>.</returns>
    private TimeType? ResolveTime()
    {
        if (Peek() != '[')
            return new TimeType();

        Advance();
        uint? hour = ParseUInt(',');
        if (hour is null)
            return null;

        uint? minute = ParseUInt(',');
        if (minute is null)
            return null;

        uint? second = ParseUInt(',');
        if (second is null)
            return null;

        uint? precision = ParseUInt(']');
        if (precision is null)
            return null;

        return new TimeType(hour, minute, second, precision);
    }

    /// <summary>
    /// Try to resolve a date-time type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved date-time type if successful else <c>null</c>.</returns>
    private DateTimeType? ResolveDateTime()
    {
        if (Peek() != '[')
            return new DateTimeType();

        Advance();
        var type = string.Empty;
        while (Peek() != '[')
            type += Advance();
        Advance();

        if (type.Trim() != "Date")
        {
            Dbir.Error(_line, $"Unable to parse DateTime with subtype {type}.");
            return null;
        }

        uint? date = ParseUInt(',');
        if (date is null)
            return null;

        uint? month = ParseUInt(',');
        if (month is null)
            return null;

        uint? year = ParseUInt(']');
        if (year is null)
            return null;

        if (Peek() != ',')
        {
            Dbir.Error(_line, "Expected a Time after Date in DateTime.");
            return null;
        }

        Advance();
        type = string.Empty;
        while (Peek() != '[')
            type += Advance();
        Advance();

        if (type.Trim() != "Time")
        {
            Dbir.Error(_line, $"Unable to parse DateTime with subtype {type}.");
            return null;
        }

        uint? hour = ParseUInt(',');
        if (hour is null)
            return null;

        uint? minute = ParseUInt(',');
        if (minute is null)
            return null;

        uint? second = ParseUInt(',');
        if (second is null)
            return null;

        uint? precision = ParseUInt(']');
        if (precision is null)
            return null;

        if (Peek() != ',')
        {
            Dbir.Error(_line, "Expected a timezone after Time in DateTime.");
            return null;
        }

        Advance();
        var value = string.Empty;
        while (Peek() != ']')
            value += Advance();
        Advance();

        if (!bool.TryParse(value, out bool timezone))
        {
            Dbir.Error(_line, $"Unable to parse {value} as a boolean.");
            return null;
        }

        DateType dateType = new(date, month, year);
        TimeType timeType = new(hour, minute, second, precision);
        return new DateTimeType(dateType, timeType, timezone);
    }

    /// <summary>
    /// Try to resolve an interval type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved interval type if successful else <c>null</c>.</returns>
    private IntervalType? ResolveInterval()
    {
        if (Peek() != '[')
            return new IntervalType();

        Advance();
        uint? hour = ParseUInt(',');
        if (hour is null)
            return null;

        uint? minute = ParseUInt(',');
        if (minute is null)
            return null;

        uint? second = ParseUInt(',');
        if (second is null)
            return null;

        uint? date = ParseUInt(',');
        if (date is null)
            return null;

        uint? month = ParseUInt(',');
        if (month is null)
            return null;

        uint? year = ParseUInt(']');
        if (year is null)
            return null;

        return new IntervalType(hour, minute, second, date, month, year);
    }

    /// <summary>
    /// Try to resolve a JSON type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved JSON type if successful else <c>null</c>.</returns>
    /// <exception cref="NotImplementedException">Complex compound type resolution has not been implemented yet.</exception>
    private JsonType ResolveJson()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to resolve a pointer type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved pointer type if successful else <c>null</c>.</returns>
    /// <exception cref="NotImplementedException">Complex compound type resolution has not been implemented yet.</exception>
    private PointerType ResolvePointer()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to resolve an option type. Logs a DBIR error if it fails.
    /// </summary>
    /// <returns>Resolved option type if successful else <c>null</c>.</returns>
    /// <exception cref="NotImplementedException">Complex compound type resolution has not been implemented yet.</exception>
    private DataType ResolveOption()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Parse an unsigned integer from the current source terminating at the peek character. Logs a DBIR error if it fails.
    /// </summary>
    /// <param name="peekChar">Character to stop parsing the unsigned integer at.</param>
    /// <returns>Parsed unsigned integer if successful else <c>null</c>.</returns>
    private uint? ParseUInt(char peekChar)
    {
        var substring = string.Empty;
        while (Peek() != peekChar)
            substring += Advance();
        Advance();

        if (uint.TryParse(substring, out uint value))
            return value;

        Dbir.Error(_line, $"Unable to parse {value} as an unsigned integer.");
        return null;
    }

    /// <summary>
    /// Add the recently parsed token from the source to the parsed tokens list.
    /// </summary>
    /// <param name="type">Type of the token just parsed.</param>
    /// <param name="literal">Value contained within the token just parsed.</param>
    private void AddToken(TokenType type, object? literal = null)
    {
        string text = source.Substring(_start, _current - _start);
        _tokens.Add(new Token(type, text, literal, _line));
    }

    /// <summary>
    /// Get the current character in the source. If at the end of the source, get the null-termination character.
    /// </summary>
    /// <returns>Current character in the source.</returns>
    private char Peek()
    {
        return AtEnd() ? '\0' : source[_current];
    }

    /// <summary>
    /// Get the current character in the source and advance the pointer to the next character.
    /// </summary>
    /// <returns>Current character in the source.</returns>
    private char Advance()
    {
        return source[_current++];
    }

    /// <summary>
    /// Continue advancing the source until the expected character is found or the source ends.
    /// </summary>
    /// <param name="expected">Expected character in the source, defaults to <c>'='</c>.</param>
    /// <returns>Whether the expected character has been found in the source.</returns>
    private bool Match(char expected = '=')
    {
        if (AtEnd())
            return false;

        if (source[_current] != expected)
            return false;

        _current++;
        return true;
    }

    /// <summary>
    /// Check whether the current pointer has reached the end of the source.
    /// </summary>
    /// <returns>Whether the pointer is at the end of the source.</returns>
    private bool AtEnd()
    {
        return _current >= source.Length;
    }
}