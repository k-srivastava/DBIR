namespace Transpiler;

/// <summary>
/// Parses a list of tokens to a list of statements and errors.
/// </summary>
/// <param name="tokens">List of tokens to be parsed into statements.</param>
public class Parser(List<Token> tokens)
{
    private Token _currentToken = tokens[0];
    private Token _nextToken = tokens[1];
    private int _currentIndex = 1;

    public readonly List<string> Errors = [];

    /// <summary>
    /// Begin parsing the tokens and generate a list of statements and errors.
    /// </summary>
    /// <returns>List of valid statements corresponding to the tokens.</returns>
    public List<Statement> ParseTokens()
    {
        List<Statement> statements = [];

        while (_nextToken.Type != TokenType.Eof)
        {
            Statement? statement = ParseStatement();
            if (statement != null)
                statements.Add(statement);
            NextToken();
        }

        return statements;
    }

    /// <summary>
    /// Parse the current token group and generate its corresponding statement.
    /// </summary>
    /// <returns></returns>
    private Statement? ParseStatement()
    {
        return _currentToken.Type switch
        {
            TokenType.Add => _nextToken.Type switch
            {
                TokenType.Column => ParseAddColumnStatement(),
                TokenType.Constraint => ParseAddConstraintStatement(),
                _ => null
            },

            TokenType.Delete => _nextToken.Type switch
            {
                TokenType.Column => ParseDeleteColumnStatement(),
                TokenType.Constraint => ParseDeleteConstraintStatement(),
                TokenType.Database => ParseDeleteDatabaseStatement(),
                TokenType.Table => ParseDeleteTableStatement(),
                _ => null
            },

            TokenType.Edit => _nextToken.Type == TokenType.Column ? ParseEditColumnStatement() : null,

            TokenType.New => _nextToken.Type switch
            {
                TokenType.Database => ParseNewDatabaseStatement(),
                TokenType.Table => ParseNewTableStatement(),
                _ => null
            },

            TokenType.Rename => _nextToken.Type switch
            {
                TokenType.Column => ParseRenameColumnStatement(),
                TokenType.Table => ParseRenameTableStatement(),
                _ => null
            },

            _ => null
        };
    }

    /// <summary>
    /// Try to parse the DDL statement to create and add a new column.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private AddColumn? ParseAddColumnStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? identifier = ParseIdentifier(baseToken.Line);
        if (identifier is null)
            return null;

        if (!ExpectNext(TokenType.LeftBrace))
            return null;

        var dataType = ParseField<IntType>("data_type", false);
        if (dataType is null)
            return null;

        var visible = ParseField<BooleanType>("visible", true, false);
        if (visible is null)
            return null;

        Token addColumn = new(TokenType.AddColumn, "Add Column", null, baseToken.Line);
        return new AddColumn(addColumn, identifier, new ColumnDefinition(dataType, visible));
    }

    /// <summary>
    /// Try to parse the DDL statement to create and add a new constraint.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    /// <exception cref="NotImplementedException"></exception>
    private AddConstraint? ParseAddConstraintStatement()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to parse the DDL statement to delete a column.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private DeleteColumn? ParseDeleteColumnStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? identifier = ParseIdentifier(baseToken.Line);
        Token deleteColumn = new(TokenType.DeleteColumn, "Delete Column", null, baseToken.Line);

        return identifier is null ? null : new DeleteColumn(deleteColumn, identifier);
    }

    /// <summary>
    /// Try to parse the DDL statement to delete a constraint.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private DeleteConstraint? ParseDeleteConstraintStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? identifier = ParseIdentifier(baseToken.Line);
        Token deleteConstraint = new(TokenType.DeleteConstraint, "Delete Constraint", null, baseToken.Line);

        return identifier is null ? null : new DeleteConstraint(deleteConstraint, identifier);
    }

    /// <summary>
    /// Try to parse the DDL statement to delete a database.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private DeleteDatabase? ParseDeleteDatabaseStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        if (!ExpectNext(TokenType.LeftParenthesis))
            return null;

        if (!ExpectNext(TokenType.Identifier))
            return null;

        string identifierLexeme = _currentToken.Lexeme;

        if (!ExpectNext(TokenType.RightParenthesis))
            return null;

        Token deleteDatabase = new(TokenType.DeleteDatabase, "Delete Database", null, baseToken.Line);
        Token identifier = new(TokenType.Identifier, identifierLexeme, null, baseToken.Line);

        return new DeleteDatabase(deleteDatabase, new Identifier(identifier));
    }

    /// <summary>
    /// Try to parse the DDL statement to delete a table.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private DeleteTable? ParseDeleteTableStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? identifier = ParseIdentifier(baseToken.Line);
        Token deleteTable = new(TokenType.DeleteTable, "Delete Table", null, baseToken.Line);

        return identifier is null ? null : new DeleteTable(deleteTable, identifier);
    }

    /// <summary>
    /// Try to parse the DDL statement to edit a column.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private EditColumn? ParseEditColumnStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? identifier = ParseIdentifier(baseToken.Line);
        if (identifier is null)
            return null;

        if (!ExpectNext(TokenType.LeftBrace))
            return null;

        var dataType = ParseField<IntType>("data_type", false);
        if (dataType is null)
            return null;

        var visible = ParseField<BooleanType>("visible", true, false);
        if (visible is null)
            return null;

        Token editColumn = new(TokenType.EditColumn, "Edit Column", null, baseToken.Line);
        return new EditColumn(editColumn, identifier, new ColumnDefinition(dataType, visible));
    }

    /// <summary>
    /// Try to parse the DDL statement to create a new database.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private NewDatabase? ParseNewDatabaseStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? identifier = ParseIdentifier(baseToken.Line);
        if (identifier is null)
            return null;

        if (!ExpectNext(TokenType.LeftBrace))
            return null;

        var password = ParseField<CharFieldType>("password");
        if (password is null)
            return null;

        var encryption = ParseField<BooleanType>("encryption");
        if (encryption is null)
            return null;

        var characterSet = ParseField<CharFieldType>("character_set");
        if (characterSet is null)
            return null;

        var timezone = ParseField<CharFieldType>("timezone");
        if (timezone is null)
            return null;

        var copyFrom = ParseField<CharFieldType>("copy_from", true, false);
        if (copyFrom is null)
            return null;

        Identifier? copyFromIdentifier = copyFrom.Value!.Length == 0
            ? null
            : new Identifier(new Token(TokenType.Identifier, string.Join("", copyFrom.Value), null, baseToken.Line));

        Token newDatabase = new(TokenType.NewDatabase, "New Database", null, baseToken.Line);
        return new NewDatabase
        (
            newDatabase, identifier,
            new DatabaseDefinition(password, encryption, characterSet, timezone, copyFromIdentifier)
        );
    }

    /// <summary>
    /// Try to parse the DDL statement to create a new database.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    /// <exception cref="NotImplementedException"></exception>
    private NewTable? ParseNewTableStatement()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to parse the DDL statement to rename a column.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private RenameColumn? ParseRenameColumnStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? oldIdentifier = ParseIdentifier(baseToken.Line);
        if (oldIdentifier is null)
            return null;

        ExpectNext(TokenType.Comma);

        Identifier? newIdentifier = ParseIdentifier(baseToken.Line);
        if (newIdentifier is null)
            return null;

        Token renameColumn = new(TokenType.RenameColumn, "Rename Column", null, baseToken.Line);
        return new RenameColumn(renameColumn, oldIdentifier, newIdentifier);
    }

    /// <summary>
    /// Try to parse the DDL statement to rename a table.
    /// </summary>
    /// <returns>Parsed statement if successful else <c>null</c>.</returns>
    private RenameTable? ParseRenameTableStatement()
    {
        Token baseToken = _currentToken;
        NextToken();

        Identifier? oldIdentifier = ParseIdentifier(baseToken.Line);
        if (oldIdentifier is null)
            return null;

        ExpectNext(TokenType.Comma);

        Identifier? newIdentifier = ParseIdentifier(baseToken.Line);
        if (newIdentifier is null)
            return null;

        Token renameTable = new(TokenType.RenameTable, "Rename Table", null, baseToken.Line);
        return new RenameTable(renameTable, oldIdentifier, newIdentifier);
    }

    /// <summary>
    /// Update the current and next token pointers to the list of tokens. When at the end, both current and next tokens point to the EOF token.
    /// </summary>
    private void NextToken()
    {
        _currentToken = _nextToken;

        if (_nextToken.Type != TokenType.Eof)
            _nextToken = tokens[++_currentIndex];
    }

    /// <summary>
    /// Check whether the next token pointer type matches the expected token type if not, an error is added. The parser advances in either case.
    /// </summary>
    /// <param name="type">Token type which next token is expected to be.</param>
    /// <returns><c>true</c> if the next token type matches the expected token type else <c>false</c>.</returns>
    private bool ExpectNext(TokenType type)
    {
        if (_nextToken.Type == type)
        {
            NextToken();
            return true;
        }

        Errors.Add($"Expected next token to be '{type}', instead got '{_nextToken.Type}'.");
        return false;
    }

    /// <summary>
    /// Parse an identifier surrounded by parenthesis and separated by dots.
    /// </summary>
    /// <param name="line">Line number on which the identifier occurs.</param>
    /// <returns>Identifier if parsing was successful else <c>null</c>.</returns>
    private Identifier? ParseIdentifier(uint line)
    {
        if (!ExpectNext(TokenType.LeftParenthesis))
            return null;

        var identifierLexeme = string.Empty;
        while (_currentToken.Type != TokenType.RightParenthesis)
        {
            if (!ExpectNext(TokenType.Identifier))
                return null;

            if (_nextToken.Type != TokenType.Dot && _nextToken.Type != TokenType.RightParenthesis)
            {
                Errors.Add($"Expected dots to separate identifier, not {_nextToken.Type}.");
                return null;
            }

            identifierLexeme += _currentToken.Lexeme + '.';
            NextToken();
        }

        return new Identifier
        (
            new Token
            (
                TokenType.Identifier,
                identifierLexeme.Remove(identifierLexeme.Length - 1), // Remove the trailing '.'.
                null, line
            )
        );
    }

    /// <summary>
    /// Parse a field and its corresponding value found in configurations for statements.
    /// </summary>
    /// <param name="fieldName">Name of the field to be parsed.</param>
    /// <param name="isInstance">Whether the data type corresponding to the field is a type or instance.</param>
    /// <param name="enforceTrailingComma">Whether to check for a trailing comma at the end of the field.</param>
    /// <typeparam name="T">Data type of the field value.</typeparam>
    /// <returns></returns>
    private T? ParseField<T>(string fieldName, bool isInstance = true, bool enforceTrailingComma = true)
        where T : DataType
    {
        if (!ExpectNext(TokenType.Identifier))
            return null;

        if (_currentToken.Lexeme != fieldName)
        {
            Errors.Add($"Expected '{fieldName}' but got '{_currentToken.Lexeme}'.");
            return null;
        }

        if (!ExpectNext(TokenType.Colon))
            return null;

        if (_nextToken.Literal is null)
        {
            Errors.Add($"Expected '{typeof(T)}' for '{fieldName}' but got null.");
            return null;
        }

        if (_nextToken.Literal is not T fieldValue)
        {
            Errors.Add($"Expected '{typeof(T)}' for '{fieldName}' but got '{_nextToken.Lexeme.GetType()}'.");
            return null;
        }

        if (isInstance)
        {
            if (!fieldValue.IsInstance())
            {
                Errors.Add($"Expected '{fieldName}' value to be an instance, not a type.");
                return null;
            }
        }

        else
        {
            if (fieldValue.IsInstance())
            {
                Errors.Add($"Expected '{fieldName}' value to be a type, not an instance.");
                return null;
            }
        }

        NextToken();

        if (enforceTrailingComma && !ExpectNext(TokenType.Comma))
            return null;

        return fieldValue;
    }
}