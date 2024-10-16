using Transpiler;

namespace TranspilerTest;

public class ParserTest
{
    [Test]
    public void ParseAddColumn()
    {
        List<Token> tokens =
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
            new(TokenType.Int32, "Int32", new IntType(IntStorage.Int32), 2),
            new(TokenType.Comma, ",", null, 2),

            new(TokenType.Identifier, "visible", null, 3),
            new(TokenType.Colon, ":", null, 3),
            new(TokenType.Boolean, "Boolean[true]", new BooleanType(true), 3),

            new(TokenType.RightBrace, "}", null, 4),
            new(TokenType.Eof, "", null, 4)
        ];

        AddColumn statement = new
        (
            new Token(TokenType.AddColumn, "Add Column", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name.column_name", null, 1)),
            new ColumnDefinition(new IntType(IntStorage.Int32), new BooleanType(true))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseAddConstraint()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void ParseDeleteColumn()
    {
        List<Token> tokens =
        [
            new(TokenType.Delete, "Delete", null, 1),
            new(TokenType.Column, "Column", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "column_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Eof, "", null, 1)
        ];

        DeleteColumn statement = new
        (
            new Token(TokenType.DeleteColumn, "Delete Column", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name.column_name", null, 1))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseDeleteConstraint()
    {
        List<Token> tokens =
        [
            new(TokenType.Delete, "Delete", null, 1),
            new(TokenType.Constraint, "Constraint", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "constraint_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Eof, "", null, 1)
        ];

        DeleteConstraint statement = new
        (
            new Token(TokenType.DeleteConstraint, "Delete Constraint", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name.constraint_name", null, 1))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseDeleteDatabase()
    {
        List<Token> tokens =
        [
            new(TokenType.Delete, "Delete", null, 1),
            new(TokenType.Database, "Database", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "database_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),
            new(TokenType.Eof, "", null, 1)
        ];

        DeleteDatabase statement = new
        (
            new Token(TokenType.DeleteDatabase, "Delete Database", null, 1),
            new Identifier(new Token(TokenType.Identifier, "database_name", null, 1))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseDeleteTable()
    {
        List<Token> tokens =
        [
            new(TokenType.Delete, "Delete", null, 1),
            new(TokenType.Table, "Table", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Eof, "", null, 1)
        ];

        DeleteTable statement = new
        (
            new Token(TokenType.DeleteTable, "Delete Table", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name", null, 1))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseEditColumn()
    {
        List<Token> tokens =
        [
            new(TokenType.Edit, "Edit", null, 1),
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
            new(TokenType.Int32, "Int32", new IntType(IntStorage.Int32), 2),
            new(TokenType.Comma, ",", null, 2),

            new(TokenType.Identifier, "visible", null, 3),
            new(TokenType.Colon, ":", null, 3),
            new(TokenType.Boolean, "Boolean[true]", new BooleanType(true), 3),

            new(TokenType.RightBrace, "}", null, 4),
            new(TokenType.Eof, "", null, 4)
        ];

        EditColumn statement = new
        (
            new Token(TokenType.EditColumn, "Edit Column", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name.column_name", null, 1)),
            new ColumnDefinition(new IntType(IntStorage.Int32), new BooleanType(true))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseNewDatabase()
    {
        List<Token> tokens =
        [
            new(TokenType.New, "New", null, 1),
            new(TokenType.Database, "Database", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "database_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),
            new(TokenType.LeftBrace, "{", null, 1),

            new(TokenType.Identifier, "password", null, 2),
            new(TokenType.Colon, ":", null, 2),
            new
            (
                TokenType.CharField, "CharField[[\"H\", \"e\", \"l\", \"l\", \"o\"], false]",
                new CharFieldType("Hello".ToCharArray()), 2
            ),
            new(TokenType.Comma, ",", null, 2),

            new(TokenType.Identifier, "encryption", null, 3),
            new(TokenType.Colon, ":", null, 3),
            new(TokenType.Boolean, "Boolean[true]", new BooleanType(true), 3),
            new(TokenType.Comma, ",", null, 3),

            new(TokenType.Identifier, "character_set", null, 4),
            new(TokenType.Colon, ":", null, 4),
            new
            (
                TokenType.CharField, "CharField[[\"U\", \"T\", \"F\", \"-\", \"8\"], false]",
                new CharFieldType("UTF-8".ToCharArray()), 4
            ),
            new(TokenType.Comma, ",", null, 4),

            new(TokenType.Identifier, "timezone", null, 5),
            new(TokenType.Colon, ":", null, 5),
            new
            (
                TokenType.CharField, "CharField[[\"I\", \"S\", \"T\"], false]", new CharFieldType("IST".ToCharArray()),
                5
            ),
            new(TokenType.Comma, ",", null, 5),

            new(TokenType.Identifier, "copy_from", null, 6),
            new(TokenType.Colon, ":", null, 6),
            new(TokenType.CharField, "CharField[[], false]", new CharFieldType([]), 6),

            new(TokenType.RightBrace, "}", null, 7),
            new(TokenType.Eof, "", null, 7)
        ];

        NewDatabase statement = new
        (
            new Token(TokenType.NewDatabase, "New Database", null, 1),
            new Identifier(new Token(TokenType.Identifier, "database_name", null, 1)),
            new DatabaseDefinition
            (
                new CharFieldType("Hello".ToCharArray()), new BooleanType(true),
                new CharFieldType("UTF-8".ToCharArray()),
                new CharFieldType("IST".ToCharArray()), null
            )
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseNewTable()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void ParseRenameColumn()
    {
        List<Token> tokens =
        [
            new(TokenType.Rename, "Rename", null, 1),
            new(TokenType.Column, "Column", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "column_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Comma, ",", null, 1),

            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "new_column_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Eof, "", null, 1)
        ];

        RenameColumn statement = new
        (
            new Token(TokenType.RenameColumn, "Rename Column", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name.column_name", null, 1)),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name.new_column_name", null, 1))
        );

        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }

    [Test]
    public void ParseRenameTable()
    {
        List<Token> tokens =
        [
            new(TokenType.Rename, "Rename", null, 1),
            new(TokenType.Table, "Table", null, 1),
            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "table_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Comma, ",", null, 1),

            new(TokenType.LeftParenthesis, "(", null, 1),
            new(TokenType.Identifier, "schema_name", null, 1),
            new(TokenType.Dot, ".", null, 1),
            new(TokenType.Identifier, "new_table_name", null, 1),
            new(TokenType.RightParenthesis, ")", null, 1),

            new(TokenType.Eof, "", null, 1)
        ];

        RenameTable statement = new
        (
            new Token(TokenType.RenameTable, "Rename Table", null, 1),
            new Identifier(new Token(TokenType.Identifier, "schema_name.table_name", null, 1)),
            new Identifier(new Token(TokenType.Identifier, "schema_name.new_table_name", null, 1))
        );
        
        Parser parser = new(tokens);
        Assert.That(new List<Statement> { statement }, Is.EqualTo(parser.ParseTokens()));
    }
}