using Transpiler;

namespace TranspilerTest;

public class Tests
{
    [Test]
    public void ResolveInt()
    {
        const string source = """
                              Int0
                              Int0[0]
                              Int8
                              Int8[-5]
                              Int16
                              Int16[12]
                              Int24
                              Int24[123]
                              Int32
                              Int32[-653]
                              Int48
                              Int48[-1230]
                              Int64
                              Int64[0987654321]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Int0, "Int0", new IntType(IntStorage.Int0), 1),
            new(TokenType.Int0, "Int0[0]", new IntType(IntStorage.Int0, 0), 2),

            new(TokenType.Int8, "Int8", new IntType(IntStorage.Int8), 3),
            new(TokenType.Int8, "Int8[-5]", new IntType(IntStorage.Int8, -5), 4),

            new(TokenType.Int16, "Int16", new IntType(IntStorage.Int16), 5),
            new(TokenType.Int16, "Int16[12]", new IntType(IntStorage.Int16, 12), 6),

            new(TokenType.Int24, "Int24", new IntType(IntStorage.Int24), 7),
            new(TokenType.Int24, "Int24[123]", new IntType(IntStorage.Int24, 123), 8),

            new(TokenType.Int32, "Int32", new IntType(IntStorage.Int32), 9),
            new(TokenType.Int32, "Int32[-653]", new IntType(IntStorage.Int32, -653), 10),

            new(TokenType.Int48, "Int48", new IntType(IntStorage.Int48), 11),
            new(TokenType.Int48, "Int48[-1230]", new IntType(IntStorage.Int48, -1230), 12),

            new(TokenType.Int64, "Int64", new IntType(IntStorage.Int64), 13),
            new(TokenType.Int64, "Int64[0987654321]", new IntType(IntStorage.Int64, 987654321), 14),

            new(TokenType.Eof, "", null, 14)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveUInt()
    {
        const string source = """
                              UInt0
                              UInt0[0]
                              UInt8
                              UInt8[5]
                              UInt16
                              UInt16[12]
                              UInt24
                              UInt24[123]
                              UInt32
                              UInt32[653]
                              UInt48
                              UInt48[1230]
                              UInt64
                              UInt64[0987654321]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.UInt0, "UInt0", new UIntType(UIntStorage.UInt0), 1),
            new(TokenType.UInt0, "UInt0[0]", new UIntType(UIntStorage.UInt0, 0), 2),

            new(TokenType.UInt8, "UInt8", new UIntType(UIntStorage.UInt8), 3),
            new(TokenType.UInt8, "UInt8[5]", new UIntType(UIntStorage.UInt8, 5), 4),

            new(TokenType.UInt16, "UInt16", new UIntType(UIntStorage.UInt16), 5),
            new(TokenType.UInt16, "UInt16[12]", new UIntType(UIntStorage.UInt16, 12), 6),

            new(TokenType.UInt24, "UInt24", new UIntType(UIntStorage.UInt24), 7),
            new(TokenType.UInt24, "UInt24[123]", new UIntType(UIntStorage.UInt24, 123), 8),

            new(TokenType.UInt32, "UInt32", new UIntType(UIntStorage.UInt32), 9),
            new(TokenType.UInt32, "UInt32[653]", new UIntType(UIntStorage.UInt32, 653), 10),

            new(TokenType.UInt48, "UInt48", new UIntType(UIntStorage.UInt48), 11),
            new(TokenType.UInt48, "UInt48[1230]", new UIntType(UIntStorage.UInt48, 1230), 12),

            new(TokenType.UInt64, "UInt64", new UIntType(UIntStorage.UInt64), 13),
            new(TokenType.UInt64, "UInt64[0987654321]", new UIntType(UIntStorage.UInt64, 987654321), 14),

            new(TokenType.Eof, "", null, 14)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveFloat()
    {
        const string source = """
                              Float32
                              Float32[345.7652]
                              Float64
                              Float64[-231233.987872348]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Float32, "Float32", new FloatType(FloatStorage.Float32), 1),
            new(TokenType.Float32, "Float32[345.7652]", new FloatType(FloatStorage.Float32, 345.7652), 2),

            new(TokenType.Float64, "Float64", new FloatType(FloatStorage.Float64), 3),
            new
            (
                TokenType.Float64, "Float64[-231233.987872348]",
                new FloatType(FloatStorage.Float64, -231233.987872348), 4
            ),

            new(TokenType.Eof, "", null, 4)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveDecimal()
    {
        const string source = """
                              Decimal
                              Decimal[-534.234, 5, 2]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Decimal, "Decimal", new DecimalType(), 1),
            new(TokenType.Decimal, "Decimal[-534.234, 5, 2]", new DecimalType(new decimal(-534.234), 5, 2), 2),
            new(TokenType.Eof, "", null, 2)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveBoolean()
    {
        const string source = """
                              Boolean
                              Boolean[true]
                              Boolean[false]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Boolean, "Boolean", new BooleanType(), 1),
            new(TokenType.Boolean, "Boolean[true]", new BooleanType(true), 2),
            new(TokenType.Boolean, "Boolean[false]", new BooleanType(false), 3),
            new(TokenType.Eof, "", null, 3)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveFields()
    {
        const string source = """
                              BitField
                              BitField[[true, false, false, true, true], false]
                              BitField[[true, false, false, true, true], true]
                              ByteField
                              ByteField[[12, 43, 65, 32], false]
                              ByteField[[12, 43, 65, 32], true]
                              CharField
                              CharField[["H", "e", "l", "l", "o"], false]
                              CharField[["H", "e", "l", "l", "o"], true]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.BitField, "BitField", new BitFieldType(), 1),
            new
            (
                TokenType.BitField, "BitField[[true, false, false, true, true], false]",
                new BitFieldType([true, false, false, true, true]), 2
            ),
            new(
                TokenType.BitField, "BitField[[true, false, false, true, true], true]",
                new BitFieldType([true, false, false, true, true], true), 3
            ),

            new(TokenType.ByteField, "ByteField", new ByteFieldType(), 4),
            new
            (
                TokenType.ByteField, "ByteField[[12, 43, 65, 32], false]", new ByteFieldType([12, 43, 65, 32]),
                5
            ),
            new
            (
                TokenType.ByteField, "ByteField[[12, 43, 65, 32], true]",
                new ByteFieldType([12, 43, 65, 32], true), 6
            ),

            new(TokenType.CharField, "CharField", new CharFieldType(), 7),
            new
            (
                TokenType.CharField, "CharField[[\"H\", \"e\", \"l\", \"l\", \"o\"], false]",
                new CharFieldType(['H', 'e', 'l', 'l', 'o']), 8
            ),
            new
            (
                TokenType.CharField, "CharField[[\"H\", \"e\", \"l\", \"l\", \"o\"], true]",
                new CharFieldType(['H', 'e', 'l', 'l', 'o'], true), 9
            ),

            new(TokenType.Eof, "", null, 9)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveDates()
    {
        const string source = """
                              Date
                              Date[23, 12, 2004]
                              Time
                              Time[10, 51, 43, 2]
                              DateTime
                              DateTime[Date[23, 12, 2004], Time[10, 51, 43, 2], false]
                              Interval
                              Interval[2, 10, 5, 3, 0, 5]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Date, "Date", new DateType(), 1),
            new(TokenType.Date, "Date[23, 12, 2004]", new DateType(23, 12, 2004), 2),

            new(TokenType.Time, "Time", new TimeType(), 3),
            new(TokenType.Time, "Time[10, 51, 43, 2]", new TimeType(10, 51, 43, 2), 4),

            new(TokenType.DateTime, "DateTime", new DateTimeType(), 5),
            new
            (
                TokenType.DateTime, "DateTime[Date[23, 12, 2004], Time[10, 51, 43, 2], false]",
                new DateTimeType(new DateType(23, 12, 2004), new TimeType(10, 51, 43, 2), false), 6
            ),

            new(TokenType.Interval, "Interval", new IntervalType(), 7),
            new
            (
                TokenType.Interval, "Interval[2, 10, 5, 3, 0, 5]",
                new IntervalType(2, 10, 5, 3, 0, 5), 8
            ),

            new(TokenType.Eof, "", null, 8)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveJson()
    {
        const string source = "JSON";

        List<Token> expectedTokens =
        [
            new(TokenType.Json, "JSON", new JsonType(), 1),
            new(TokenType.Eof, "", null, 2)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolvePointer()
    {
        const string source = """
                              Pointer[Int32]
                              Pointer[Int32[123]]
                              Pointer[Pointer[Int32]]
                              Pointer[Pointer[Int32[123]]]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Pointer, "Pointer[Int32]", new PointerType(new IntType(IntStorage.Int32)), 1),
            new(TokenType.Pointer, "Pointer[Int32[123]]", new PointerType(new IntType(IntStorage.Int32, 123)), 2),

            new
            (
                TokenType.Pointer, "Pointer[Pointer[Int32]]",
                new PointerType(new PointerType(new IntType(IntStorage.Int32))), 3
            ),
            new
            (
                TokenType.Pointer, "Pointer[Pointer[Int32[123]]]",
                new PointerType(new PointerType(new IntType(IntStorage.Int32, 123))), 4
            ),

            new(TokenType.Eof, "", null, 5)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveOption()
    {
        const string source = """
                              Option[Int32]
                              Option[Option[Int32]]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Option, "Option[Int32]", new OptionType(new IntType(IntStorage.Int32)), 1),
            new(
                TokenType.Option, "Option[Option[Int32]]",
                new OptionType(new OptionType(new IntType(IntStorage.Int32, 123))), 2
            ),
            new(TokenType.Eof, "", null, 3)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveSome()
    {
        const string source = """
                              Some[Int32[123]]
                              Some[Some[Int32[123]]
                              Some[None]
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Some, "Some[Int32[123]]", new SomeType(new IntType(IntStorage.Int32)), 1),
            new
            (
                TokenType.Some, "Some[Some[Int32[123]]]",
                new SomeType(new SomeType(new IntType(IntStorage.Int32, 123))), 2
            ),
            new(TokenType.Some, "Some[None]", new SomeType(new NoneType()), 3),
            new(TokenType.Eof, "", null, 4)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void ResolveNone()
    {
        const string source = "None";
        List<Token> expectedTokens =
        [
            new(TokenType.None, "None", new NoneType(), 1),
            new(TokenType.Eof, "", null, 1)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }

    [Test]
    public void AddColumn()
    {
        const string source = """
                              Add Column(schema_name.table_name.column_name) {
                                  data_type: Int32[456],
                                  visible: None
                              }
                              """;

        List<Token> expectedTokens =
        [
            new(TokenType.Add, "Add", null, 1),
            new(TokenType.Column, "Column", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "column_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),
            new(TokenType.LeftBrace, "{", null, 1),
            new(TokenType.Identifier, "data_type", null, 2),
            new(TokenType.Colon, ":", null, 2),
            new(TokenType.Int32, "Int32[456]", new IntType(IntStorage.Int32, 456), 2),
            new(TokenType.Comma, ",", null, 2),
            new(TokenType.Identifier, "visible", null, 3),
            new(TokenType.Colon, ":", null, 3),
            new(TokenType.None, "None", new NoneType(), 3),
            new(TokenType.RightBrace, "}", null, 4),
            new(TokenType.Eof, "", null, 4)
        ];

        Scanner scanner = new(source);
        Assert.That(expectedTokens, Is.EqualTo(scanner.ScanTokens()));
    }
}