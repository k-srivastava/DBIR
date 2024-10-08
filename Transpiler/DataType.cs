using System.Diagnostics;

namespace Transpiler;

/// <summary>
///     Abstract data type class representing all data types supported in the language.
/// </summary>
public abstract class DataType;

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
/// <param name="storage">Storage size of the integer.</param>
/// <param name="value">Value held by the integer, if any.</param>
public class IntType(IntStorage storage, long? value = null) : DataType
{
    public readonly IntStorage Storage = storage;
    public readonly long? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is IntType other)
            return Equals(other);
        return false;
    }

    private bool Equals(IntType other)
    {
        return Storage == other.Storage && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Storage, Value);
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
/// <param name="storage">Storage size of the unsigned integer.</param>
/// <param name="value">Value held by the unsigned integer, if any.</param>
public class UIntType(UIntStorage storage, ulong? value = null) : DataType
{
    public readonly UIntStorage Storage = storage;
    public readonly ulong? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is UIntType other)
            return Equals(other);
        return false;
    }

    private bool Equals(UIntType other)
    {
        return Storage == other.Storage && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Storage, Value);
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
/// <param name="storage">Storage size of the floating-point number.</param>
/// <param name="value">Value held by the floating-pointer number, if any.</param>
public class FloatType(FloatStorage storage, double? value = null) : DataType
{
    private const double ComparisonDelta = 0.000001;
    public readonly FloatStorage Storage = storage;
    public readonly double? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is FloatType other)
            return Equals(other);
        return false;
    }

    private bool Equals(FloatType other)
    {
        if (Storage != other.Storage)
            return false;

        if (Value is null && other.Value is null)
            return true;

        if (Value is null || other.Value is null)
            return false;

        // Both Value and other.Value should not be null at this stage.
        Debug.Assert(Value is not null, $"{nameof(Value)} != null");
        Debug.Assert(other.Value is not null, $"{nameof(other.Value)} != null");

        // Coalesce to 0 to avoid compiler errors. 
        double delta = (Value ?? 0) - (other.Value ?? 0);
        return Math.Abs(delta) <= ComparisonDelta;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Storage, Value);
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
/// <param name="value">Value held by the decimal number.</param>
/// <param name="precision">Precision of the decimal number.</param>
/// <param name="scale">Scale of the decimal number.</param>
public class DecimalType(decimal? value = null, uint? precision = null, uint? scale = null) : DataType
{
    public readonly uint? Precision = precision;
    public readonly uint? Scale = scale;
    public readonly decimal? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is DecimalType other)
            return Equals(other);
        return false;
    }

    private bool Equals(DecimalType other)
    {
        return Precision == other.Precision && Scale == other.Scale && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Precision, Scale, Value);
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
/// <param name="value">Value help by the boolean, if any.</param>
public class BooleanType(bool? value = null) : DataType
{
    public readonly bool? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is BooleanType other)
            return Equals(other);
        return false;
    }

    private bool Equals(BooleanType other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value is null ? "Boolean[]" : $"Boolean[Value: {Value}]";
    }
}

/// <summary>
///     Field type containing bits represented internally as <c>bool</c>s in an array.
/// </summary>
/// <param name="value">Value of the bit field, if any.</param>
/// <param name="resizable">Whether the bit field is resizable after allocation, defaults to <c>false</c>.</param>
public class BitFieldType(bool[]? value = null, bool resizable = false) : DataType
{
    public readonly bool Resizable = resizable;
    public readonly bool[]? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is BitFieldType other)
            return Equals(other);
        return false;
    }

    private bool Equals(BitFieldType other)
    {
        if (Resizable != other.Resizable)
            return false;

        if (Value is null && other.Value is null)
            return true;

        if (Value is null || other.Value is null)
            return false;

        return Value.SequenceEqual(other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Resizable, Value);
    }
}

/// <summary>
///     Field type containing bytes represented internally as <c>byte</c>s in an array.
/// </summary>
/// <param name="value">Value of the byte field, if any.</param>
/// <param name="resizable">Whether the byte field is resizable after allocation, defaults to <c>false</c>.</param>
public class ByteFieldType(byte[]? value = null, bool resizable = false) : DataType
{
    public readonly bool Resizable = resizable;
    public readonly byte[]? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is ByteFieldType other)
            return Equals(other);
        return false;
    }

    private bool Equals(ByteFieldType other)
    {
        if (Resizable != other.Resizable)
            return false;

        if (Value is null && other.Value is null)
            return true;

        if (Value is null || other.Value is null)
            return false;

        return Value.SequenceEqual(other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Resizable, Value);
    }
}

/// <summary>
///     Field type containing characters represented internally as <c>char</c>s in an array.
/// </summary>
/// <param name="value">Value of the char field, if any.</param>
/// <param name="resizable">Whether the char field is resizable after allocation, defaults to <c>false</c>.</param>
public class CharFieldType(char[]? value = null, bool resizable = false) : DataType
{
    public readonly bool Resizable = resizable;
    public readonly char[]? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is CharFieldType other)
            return Equals(other);
        return false;
    }

    private bool Equals(CharFieldType other)
    {
        if (Resizable != other.Resizable)
            return false;

        if (Value is null && other.Value is null)
            return true;

        if (Value is null || other.Value is null)
            return false;

        return Value.SequenceEqual(other.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Resizable, Value);
    }
}

/// <summary>
///     Date type containing the date, month and year for a Gregorian calendar date.
/// </summary>
/// <param name="date">Date, ranges from 1 to 31.</param>
/// <param name="month">Month, ranges form 1 to 12.</param>
/// <param name="year">Year, range depends on SQL provider.</param>
public class DateType(uint? date = null, uint? month = null, uint? year = null) : DataType
{
    public readonly uint? Date = date;
    public readonly uint? Month = month;
    public readonly uint? Year = year;

    public override bool Equals(object? obj)
    {
        if (obj is DateType other)
            return Equals(other);
        return false;
    }

    private bool Equals(DateType other)
    {
        return Date == other.Date && Month == other.Month && Year == other.Year;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Month, Year);
    }
}

/// <summary>
///     Time type containing the hour, minute, second and precision in a 24-hour format.
/// </summary>
/// <param name="hour">Hour, ranges from 0 to 23.</param>
/// <param name="minute">Minute, ranges from 0 to 59.</param>
/// <param name="second">Second, ranges from 0 to 59.</param>
/// <param name="precision">Precision of the time calculation.</param>
public class TimeType(uint? hour = null, uint? minute = null, uint? second = null, uint? precision = null) : DataType
{
    public readonly uint? Hour = hour;
    public readonly uint? Minute = minute;
    public readonly uint? Precision = precision;
    public readonly uint? Second = second;

    public override bool Equals(object? obj)
    {
        if (obj is TimeType other)
            return Equals(other);
        return false;
    }

    private bool Equals(TimeType other)
    {
        return Hour == other.Hour && Minute == other.Minute && Precision == other.Precision && Second == other.Second;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hour, Minute, Precision, Second);
    }
}

/// <summary>
///     Date and time combined into a single type with a timezone.
/// </summary>
/// <param name="date">Date type.</param>
/// <param name="time">Time type.</param>
/// <param name="timezone">Whether the date and time belong to a timezone.</param>
public class DateTimeType(DateType? date = null, TimeType? time = null, bool? timezone = null) : DataType
{
    public readonly DateType? Date = date;
    public readonly TimeType? Time = time;
    public readonly bool? Timezone = timezone;

    public override bool Equals(object? obj)
    {
        if (obj is DateTimeType other)
            return Equals(other);
        return false;
    }

    private bool Equals(DateTimeType other)
    {
        return Equals(Date, other.Date) && Timezone == other.Timezone && Equals(Time, other.Time);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Timezone, Time);
    }
}

/// <summary>
///     Interval between any two dates or times.
/// </summary>
/// <param name="hour">Delta of hours, ranges from 0 to 23.</param>
/// <param name="minute">Delta of minutes, ranges from 0 to 59.</param>
/// <param name="second">Delta of seconds, ranges from 0 to 59.</param>
/// <param name="date">Delta of dates, ranges from 0 to 31.</param>
/// <param name="month">Delta of months, ranges from 0 to 12.</param>
/// <param name="year">Delta of years, range depends on SQL provider.</param>
public class IntervalType(
    uint? hour = null,
    uint? minute = null,
    uint? second = null,
    uint? date = null,
    uint? month = null,
    uint? year = null
) : DataType
{
    public readonly uint? Date = date;
    public readonly uint? Hour = hour;
    public readonly uint? Minute = minute;
    public readonly uint? Month = month;
    public readonly uint? Second = second;
    public readonly uint? Year = year;

    public override bool Equals(object? obj)
    {
        if (obj is IntervalType other)
            return Equals(other);
        return false;
    }

    private bool Equals(IntervalType other)
    {
        return Date == other.Date && Hour == other.Hour && Minute == other.Minute && Month == other.Month &&
               Second == other.Second && Year == other.Year;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Hour, Minute, Month, Second, Year);
    }
}

/// <summary>
///     Represents a JSON string object.
/// </summary>
/// <param name="value">JSON parsable string data.</param>
public class JsonType(string? value = null) : DataType
{
    public readonly string? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is JsonType other)
            return Equals(other);
        return false;
    }

    private bool Equals(JsonType other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value != null ? Value.GetHashCode() : 0;
    }
}

/// <summary>
///     Represents the lack of value for an <c>Option</c> type. All <c>None</c> types are inherently equal to each other.
/// </summary>
public class NoneType() : OptionType<NoneType>(null)
{
    public override bool Equals(object? obj)
    {
        return obj is NoneType;
    }

    public override int GetHashCode()
    {
        return GetType().GetHashCode();
    }

    public override string ToString()
    {
        return "None";
    }
}

/// <summary>
///     Represents the presence of value for an <c>Option</c> type which could be any other <c>DataType</c>.
/// </summary>
/// <param name="value">Value contained within the type.</param>
public class SomeType<T>(T value) : OptionType<T>(value) where T: DataType
{
    public readonly new T Value = value;
    
    public override bool Equals(object? obj)
    {
        if (obj is SomeType<T> other)
            return Equals(other);
        return false;
    }

    private bool Equals(SomeType<T> other)
    {
        return Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"Some[Value: {Value}]";
    }
}

/// <summary>
///     Represents an optional value container over any other <c>DataType</c>.
/// </summary>
/// <param name="value">Value represented by the type that could potentially exist.</param>
public class OptionType<T>(T? value) : DataType where T: DataType
{
    public readonly T? Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is OptionType<T> other)
            return Equals(other);
        return false;
    }

    private bool Equals(OptionType<T> other)
    {
        return Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return Value != null ? Value.GetHashCode() : 42;
    }

    public override string ToString()
    {
        return $"Option[Value: {Value}]";
    }
}

/// <summary>
///     Represents a pointer container over any other <c>DataType</c>.
/// </summary>
/// <param name="value">Value pointed to by the pointer.</param>
public class PointerType(DataType value) : DataType
{
    public readonly DataType Value = value;

    public override bool Equals(object? obj)
    {
        if (obj is PointerType other)
            return Equals(other);
        return false;
    }

    private bool Equals(PointerType other)
    {
        return Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}