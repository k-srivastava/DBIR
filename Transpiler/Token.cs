namespace Transpiler;

/// <summary>
///     Enum for all the types of tokens supported by the scanner.
/// </summary>
public enum TokenType
{
    // @formatter:off
    LeftParenthesis, RightParenthesis,
    LeftBrace, RightBrace,
    LeftBracket, RightBracket,
    Comma, Dot, Colon,
    
    Plus, Minus, Asterisk, Slash, Percent,
    
    Bang, BangEquals,
    Equal, EqualEquals,
    GreaterThan, GreaterThanEquals,
    LesserThan, LesserThanEquals,
    
    Except, ExceptAll,
    Intersect, IntersectAll,
    Union, UnionAll,
    
    Column, Table, Tables, Constraint, Database, Records,
    
    Add, Delete, Edit, Update, Rename, New, Insert, Select, Join,
    
    AddColumn, DeleteColumn, EditColumn, RenameColumn,
    AddConstraint, DeleteConstraint,
    DeleteDatabase, NewDatabase,
    DeleteTable, NewTable, RenameTable,
    
    Int0, Int8, Int16, Int24, Int32, Int48, Int64,
    UInt0, UInt8, UInt16, UInt24, UInt32, UInt48, UInt64,
    Float32, Float64, Decimal,
    Boolean,
    BitField, ByteField, CharField,
    Date, Time, DateTime, Interval,
    Json,
    Pointer,
    Option, Some, None,
    
    Identifier,
    Eof
    // @formatter:on
}

/// <summary>
///     Represents the smallest singular unit of code in DBIR.
/// </summary>
/// <param name="Type">Type of the token.</param>
/// <param name="Lexeme">String representing the token.</param>
/// <param name="Literal">Data contained within the token.</param>
/// <param name="Line">Line of code in which the token occurs.</param>
public record Token(TokenType Type, string Lexeme, object? Literal, uint Line)
{
    /// <summary>
    ///     Get the corresponding token type from an identifier string.
    /// </summary>
    /// <param name="identifier">Case-sensitive identifier string corresponding to a token type.</param>
    /// <returns>Corresponding token type if it matches or <c>TokenType.Identifier</c>.</returns>
    public static TokenType LookupIdentifier(string identifier)
    {
        var keywords = new Dictionary<string, TokenType>
        {
            { "except", TokenType.Except },
            { "except_all", TokenType.ExceptAll },
            { "intersect", TokenType.Intersect },
            { "intersect_all", TokenType.IntersectAll },
            { "union", TokenType.Union },
            { "union_all", TokenType.UnionAll },

            { "Column", TokenType.Column },
            { "Table", TokenType.Table },
            { "Tables", TokenType.Tables },
            { "Constraint", TokenType.Constraint },
            { "Database", TokenType.Database },
            { "Records", TokenType.Records },

            { "Add", TokenType.Add },
            { "Delete", TokenType.Delete },
            { "Edit", TokenType.Edit },
            { "Update", TokenType.Update },
            { "Rename", TokenType.Rename },
            { "New", TokenType.New },
            { "Insert", TokenType.Insert },
            { "Select", TokenType.Select },
            { "Join", TokenType.Join },

            { "Int0", TokenType.Int0 },
            { "Int8", TokenType.Int8 },
            { "Int16", TokenType.Int16 },
            { "Int24", TokenType.Int24 },
            { "Int32", TokenType.Int32 },
            { "Int48", TokenType.Int48 },
            { "Int64", TokenType.Int64 },

            { "UInt0", TokenType.UInt0 },
            { "UInt8", TokenType.UInt8 },
            { "UInt16", TokenType.UInt16 },
            { "UInt24", TokenType.UInt24 },
            { "UInt32", TokenType.UInt32 },
            { "UInt48", TokenType.UInt48 },
            { "UInt64", TokenType.UInt64 },

            { "Float32", TokenType.Float32 },
            { "Float64", TokenType.Float64 },
            { "Decimal", TokenType.Decimal },

            { "Boolean", TokenType.Boolean },

            { "BitField", TokenType.BitField },
            { "ByteField", TokenType.ByteField },
            { "CharField", TokenType.CharField },

            { "Date", TokenType.Date },
            { "Time", TokenType.Time },
            { "DateTime", TokenType.DateTime },
            { "Interval", TokenType.Interval },

            { "JSON", TokenType.Json },
            { "Pointer", TokenType.Pointer },

            { "Option", TokenType.Option },
            { "Some", TokenType.Some },
            { "None", TokenType.None }
        };

        return keywords.GetValueOrDefault(identifier, TokenType.Identifier);
    }

    /// <summary>
    ///     Check whether a particular lexeme corresponds to a data type in the language.
    /// </summary>
    /// <param name="lexeme">String that could represent a data type.</param>
    /// <returns><c>true</c> if the lexeme refers to a data type else <c>false</c>.</returns>
    public static bool IsDataType(string lexeme)
    {
        string[] dataTypes =
        [
            "Int0", "Int8", "Int16", "Int24", "Int32", "Int48", "Int64", "UInt0", "UInt8", "UInt16", "UInt24", "UInt32",
            "UInt48", "UInt64", "Float32", "Float64", "Decimal", "Boolean", "BitField", "ByteField", "CharField",
            "Date", "Time", "DateTime", "Interval", "JSON", "Pointer", "Option", "Some", "None"
        ];

        return dataTypes.Contains(lexeme);
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal} | {Line}";
    }
}