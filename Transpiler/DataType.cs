namespace Transpiler;

/// <summary>
///     Abstract data type representing all data types supported in the language.
/// </summary>
public abstract record DataType
{
    /// <summary>
    ///     Check if the data type is a type or an instance of the type.
    /// </summary>
    /// <returns>Whether the data type is an instance of the type, or not.</returns>
    public abstract bool IsInstance();
}

/// <summary>
///     All possible storage sizes for the <c>IntType</c> data type. Ranges from 0 to 64 bits in size.
/// </summary>
public enum IntStorage
{
    Int0,
    Int8,
    Int16,
    Int24,
    Int32,
    Int48,
    Int64
}

/// <summary>
///     Signed integer data type.
/// </summary>
/// <param name="Storage">Storage size of the integer.</param>
/// <param name="Value">Value held by the integer, if any.</param>
public record IntType(IntStorage Storage, long? Value = null) : DataType
{
    public override bool IsInstance()
    {
        return Value.HasValue;
    }

    public override string ToString()
    {
        return Value is null ? $"Int[Storage: {Storage}]" : $"Int[Storage: {Storage}, Value: {Value}]";
    }

    /// <summary>
    ///     Get the corresponding integer storage size from its token type.
    /// </summary>
    /// <param name="type">Token type corresponding to the integer storage size.</param>
    /// <returns>Corresponding integer storage type.</returns>
    /// <exception cref="ArgumentException">Token type does not correspond with any integer storage type.</exception>
    public static IntStorage GetStorageFromTokenType(TokenType type)
    {
        Dictionary<TokenType, IntStorage> typeToStorage = new()
        {
            { TokenType.Int0, IntStorage.Int0 },
            { TokenType.Int8, IntStorage.Int8 },
            { TokenType.Int16, IntStorage.Int16 },
            { TokenType.Int24, IntStorage.Int24 },
            { TokenType.Int32, IntStorage.Int32 },
            { TokenType.Int48, IntStorage.Int48 },
            { TokenType.Int64, IntStorage.Int64 }
        };

        if (!typeToStorage.TryGetValue(type, out IntStorage storage))
            throw new ArgumentException($"Unknown integer storage token type: {type}.");

        return storage;
    }
}

/// <summary>
///     All possible storage sizes for the <c>UIntType</c> data type. Ranges from 0 to 64 bits in size.
/// </summary>
public enum UIntStorage
{
    UInt0,
    UInt8,
    UInt16,
    UInt24,
    UInt32,
    UInt48,
    UInt64
}

/// <summary>
///     Unsigned integer data type.
/// </summary>
/// <param name="Storage">Storage size of the unsigned integer.</param>
/// <param name="Value">Value held by the unsigned integer, if any.</param>
public record UIntType(UIntStorage Storage, ulong? Value = null) : DataType
{
    public override bool IsInstance()
    {
        return Value.HasValue;
    }

    public override string ToString()
    {
        return Value is null ? $"UInt[Storage: {Storage}]" : $"UInt[Storage: {Storage}, Value: {Value}]";
    }

    /// <summary>
    ///     Get the corresponding unsigned integer storage size from its token type.
    /// </summary>
    /// <param name="type">Token type corresponding to the unsigned integer storage size.</param>
    /// <returns>Corresponding unsigned integer storage type.</returns>
    /// <exception cref="ArgumentException">Token type does not correspond with any unsigned integer storage type.</exception>
    public static UIntStorage GetStorageFromTokenType(TokenType type)
    {
        Dictionary<TokenType, UIntStorage> typeToStorage = new()
        {
            { TokenType.UInt0, UIntStorage.UInt0 },
            { TokenType.UInt8, UIntStorage.UInt8 },
            { TokenType.UInt16, UIntStorage.UInt16 },
            { TokenType.UInt24, UIntStorage.UInt24 },
            { TokenType.UInt32, UIntStorage.UInt32 },
            { TokenType.UInt48, UIntStorage.UInt48 },
            { TokenType.UInt64, UIntStorage.UInt64 }
        };

        if (!typeToStorage.TryGetValue(type, out UIntStorage storage))
            throw new ArgumentException($"Unknown unsigned integer storage token type: {type}.");

        return storage;
    }
}

/// <summary>
///     All possible storage sizes for the <c>FloatType</c> data type. Ranges from 32 to 64 bits in size.
/// </summary>
public enum FloatStorage
{
    Float32,
    Float64
}

/// <summary>
///     Floating-point number data type.
/// </summary>
/// <param name="Storage">Storage size of the floating-point number.</param>
/// <param name="Value">Value held by the floating-pointer number, if any.</param>
public record FloatType(FloatStorage Storage, double? Value = null) : DataType
{
    public override bool IsInstance()
    {
        return Value.HasValue;
    }

    public override string ToString()
    {
        return Value is null ? $"Float[Storage: {Storage}]" : $"Float[Storage: {Storage}, Value: {Value}]";
    }

    /// <summary>
    ///     Get the corresponding floating-pointer number storage size from its token type.
    /// </summary>
    /// <param name="type">Token type corresponding to the floating-pointer number storage size.</param>
    /// <returns>Corresponding floating-pointer number storage type.</returns>
    /// <exception cref="ArgumentException">Token type does not correspond with any floating-pointer number storage type.</exception>
    public static FloatStorage GetStorageFromTokenType(TokenType type)
    {
        Dictionary<TokenType, FloatStorage> typeToStorage = new()
        {
            { TokenType.Float32, FloatStorage.Float32 },
            { TokenType.Float64, FloatStorage.Float64 }
        };

        if (!typeToStorage.TryGetValue(type, out FloatStorage storage))
            throw new ArgumentException($"Unknown floating-point number storage token type: {type}.");

        return storage;
    }
}

/// <summary>
///     Decimal number data type.
/// </summary>
/// <param name="Value">Value held by the decimal number.</param>
/// <param name="Precision">Precision of the decimal number.</param>
/// <param name="Scale">Scale of the decimal number.</param>
public record DecimalType(decimal? Value = null, uint? Precision = null, uint? Scale = null) : DataType
{
    public override bool IsInstance()
    {
        return Value.HasValue && Precision.HasValue && Scale.HasValue;
    }

    public override string ToString()
    {
        if (Value is null && Precision is null && Scale is null)
            return "Decimal[]";
        return $"Decimal[Value: {Value}, Precision: {Precision}, Scale: {Scale}]";
    }
}

/// <summary>
///     Boolean data type.
/// </summary>
/// <param name="Value">Value help by the boolean, if any.</param>
public record BooleanType(bool? Value = null) : DataType
{
    public override bool IsInstance()
    {
        return Value.HasValue;
    }

    public override string ToString()
    {
        return Value is null ? "Boolean[]" : $"Boolean[Value: {Value}]";
    }
}

/// <summary>
///     Field type containing bits represented internally as <c>bool</c>s in an array.
/// </summary>
/// <param name="Value">Value of the bit field, if any.</param>
/// <param name="Resizable">Whether the bit field is resizable after allocation, defaults to <c>false</c>.</param>
public record BitFieldType(bool[]? Value = null, bool Resizable = false) : DataType
{
    public virtual bool Equals(BitFieldType? other)
    {
        if (other is null)
            return false;

        if (other.Resizable != Resizable)
            return false;

        if (Value is null && other.Value is null)
            return true;

        return Value!.SequenceEqual(other.Value!);
    }

    public override bool IsInstance()
    {
        return Value is not null;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value, Resizable);
    }
}

/// <summary>
///     Field type containing bytes represented internally as <c>byte</c>s in an array.
/// </summary>
/// <param name="Value">Value of the byte field, if any.</param>
/// <param name="Resizable">Whether the byte field is resizable after allocation, defaults to <c>false</c>.</param>
public record ByteFieldType(byte[]? Value = null, bool Resizable = false) : DataType
{
    public virtual bool Equals(ByteFieldType? other)
    {
        if (other is null)
            return false;

        if (other.Resizable != Resizable)
            return false;

        if (Value is null && other.Value is null)
            return true;

        return Value!.SequenceEqual(other.Value!);
    }

    public override bool IsInstance()
    {
        return Value is not null;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value, Resizable);
    }
}

/// <summary>
///     Field type containing characters represented internally as <c>char</c>s in an array.
/// </summary>
/// <param name="Value">Value of the char field, if any.</param>
/// <param name="Resizable">Whether the char field is resizable after allocation, defaults to <c>false</c>.</param>
public record CharFieldType(char[]? Value = null, bool Resizable = false) : DataType
{
    public virtual bool Equals(CharFieldType? other)
    {
        if (other is null)
            return false;

        if (other.Resizable != Resizable)
            return false;

        if (Value is null && other.Value is null)
            return true;

        return Value!.SequenceEqual(other.Value!);
    }

    public override bool IsInstance()
    {
        return Value is not null;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value, Resizable);
    }

    public override string ToString()
    {
        return IsInstance()
            ? $"CharField[\"{string.Join("", Value!)}\", Resizable: {Resizable}]"
            : $"CharField[Resizable: {Resizable}]";
    }
}

/// <summary>
///     Date type containing the date, month and year for a Gregorian calendar date.
/// </summary>
/// <param name="Date">Date, ranges from 1 to 31.</param>
/// <param name="Month">Month, ranges form 1 to 12.</param>
/// <param name="Year">Year, range depends on SQL provider.</param>
public record DateType(uint? Date = null, uint? Month = null, uint? Year = null) : DataType
{
    public override bool IsInstance()
    {
        return Date.HasValue && Month.HasValue && Year.HasValue;
    }
}

/// <summary>
///     Time type containing the hour, minute, second and precision in a 24-hour format.
/// </summary>
/// <param name="Hour">Hour, ranges from 0 to 23.</param>
/// <param name="Minute">Minute, ranges from 0 to 59.</param>
/// <param name="Second">Second, ranges from 0 to 59.</param>
/// <param name="Precision">Precision of the time calculation.</param>
public record TimeType(uint? Hour = null, uint? Minute = null, uint? Second = null, uint? Precision = null) : DataType
{
    public override bool IsInstance()
    {
        return Hour.HasValue && Minute.HasValue && Second.HasValue && Precision.HasValue;
    }
}

/// <summary>
///     Date and time combined into a single type with a timezone.
/// </summary>
/// <param name="Date">Date type.</param>
/// <param name="Time">Time type.</param>
/// <param name="Timezone">Whether the date and time belong to a timezone.</param>
public record DateTimeType(DateType? Date = null, TimeType? Time = null, bool? Timezone = null) : DataType
{
    public override bool IsInstance()
    {
        if (!Timezone.HasValue)
            return false;

        if (Date is not null && Time is not null)
            return Date.IsInstance() && Time.IsInstance();

        return !(Date is null && Time is null);
    }
}

/// <summary>
///     Interval between any two dates or times.
/// </summary>
/// <param name="Hour">Delta of hours, ranges from 0 to 23.</param>
/// <param name="Minute">Delta of minutes, ranges from 0 to 59.</param>
/// <param name="Second">Delta of seconds, ranges from 0 to 59.</param>
/// <param name="Date">Delta of dates, ranges from 0 to 31.</param>
/// <param name="Month">Delta of months, ranges from 0 to 12.</param>
/// <param name="Year">Delta of years, range depends on SQL provider.</param>
public record IntervalType(
    uint? Hour = null,
    uint? Minute = null,
    uint? Second = null,
    uint? Date = null,
    uint? Month = null,
    uint? Year = null
) : DataType
{
    public override bool IsInstance()
    {
        return Hour.HasValue && Minute.HasValue && Second.HasValue && Date.HasValue && Month.HasValue && Year.HasValue;
    }
}

/// <summary>
///     Represents a JSON string object.
/// </summary>
/// <param name="Value">JSON parsable string data.</param>
public record JsonType(string? Value = null) : DataType
{
    public override bool IsInstance()
    {
        return Value is not null;
    }
}

/// <summary>
///     Represents the lack of value for an <c>Option</c> type. All <c>None</c> types are inherently equal to each other.
/// </summary>
public record NoneType() : OptionType<NoneType>(null);

/// <summary>
///     Represents the presence of value for an <c>Option</c> type which could be any other <c>DataType</c>.
/// </summary>
/// <param name="Value">Value contained within the type.</param>
/// <typeparam name="T">Data type encapsulated within the some.</typeparam>
public record SomeType<T>(T Value) : OptionType<T>(Value) where T : DataType
{
    public override string ToString()
    {
        return $"Some[Value: {Value}]";
    }
}

/// <summary>
///     Represents an optional value container over any other <c>DataType</c>.
/// </summary>
/// <param name="Value">Value represented by the type that could potentially exist.</param>
/// <typeparam name="T">Data type encapsulated within the option.</typeparam>
public record OptionType<T>(T? Value) : DataType where T : DataType
{
    public override bool IsInstance()
    {
        return Value is not null;
    }

    public override string ToString()
    {
        return $"Option[Value: {Value}]";
    }
}

/// <summary>
///     Represents a pointer container over any other <c>DataType</c>.
/// </summary>
/// <param name="Value">Value pointed to by the pointer.</param>
public record PointerType(DataType Value) : DataType
{
    public override bool IsInstance()
    {
        return Value.IsInstance();
    }
}