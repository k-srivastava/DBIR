namespace Transpiler;

/// <summary>
///     General statement node supported by DBIR.
/// </summary>
/// <param name="Token">Token corresponding to the statement.</param>
public abstract record Statement(Token Token)
{
    public abstract override string ToString();
}

/// <summary>
///     DDL statement to create and add a new column to an existing table.
/// </summary>
/// <param name="Token">Add column token details.</param>
/// <param name="Name">Name of the column to be created and added.</param>
/// <param name="Definition">Column configurations.</param>
public sealed record AddColumn(Token Token, Identifier Name, ColumnDefinition Definition) : Statement(Token)
{
    public override string ToString()
    {
        return $"Add Column ({Name}) {{ {Definition} }}";
    }
}

/// <summary>
///     DDL statement to delete an existing column in an existing table.
/// </summary>
/// <param name="Token">Delete column token details.</param>
/// <param name="Name">Name of the column to be deleted.</param>
public sealed record DeleteColumn(Token Token, Identifier Name) : Statement(Token)
{
    public override string ToString()
    {
        return $"Delete Column ({Name})";
    }
}

/// <summary>
///     DDL statement to edit an existing column in an existing table.
/// </summary>
/// <param name="Token">Edit column token details.</param>
/// <param name="Name">Name of the column to be edited.</param>
/// <param name="Definition">Column configurations.</param>
public sealed record EditColumn(Token Token, Identifier Name, ColumnDefinition Definition) : Statement(Token)
{
    public override string ToString()
    {
        return $"Edit Column ({Name}) {{ {Definition} }}";
    }
}

/// <summary>
///     DDL statement to rename an existing column in an existing table.
/// </summary>
/// <param name="Token">Rename column token details.</param>
/// <param name="OriginalName">Original name of the column to be renamed.</param>
/// <param name="NewName">New name of the column to be renamed.</param>
public sealed record RenameColumn(Token Token, Identifier OriginalName, Identifier NewName) : Statement(Token)
{
    public override string ToString()
    {
        return $"Rename Column ({OriginalName}), ({NewName})";
    }
}

/// <summary>
///     DDL statement to create and add a new constraint to an existing table.
/// </summary>
/// <param name="Token">Add constraint token details.</param>
/// <param name="Name">Name of the constraint to be created and added.</param>
/// <param name="Definition">Constraint configurations.</param>
public sealed record AddConstraint(Token Token, Identifier Name, ConstraintDefinition Definition) : Statement(Token)
{
    public override string ToString()
    {
        return $"Add Constraint ({Name}) {{ {Definition} }}";
    }
}

/// <summary>
///     DDL statement to delete an existing constraint in an existing table.
/// </summary>
/// <param name="Token">Delete constraint token details.</param>
/// <param name="Name">Name of the constraint to be deleted.</param>
public sealed record DeleteConstraint(Token Token, Identifier Name) : Statement(Token)
{
    public override string ToString()
    {
        return $"Delete Constraint ({Name})";
    }
}

/// <summary>
///     DDL statement to delete an existing database.
/// </summary>
/// <param name="Token">Delete database token details.</param>
/// <param name="Name">Name of the database to be deleted.</param>
public sealed record DeleteDatabase(Token Token, Identifier Name) : Statement(Token)
{
    public override string ToString()
    {
        return $"Delete Database ({Name})";
    }
}

/// <summary>
///     DDL statement to create a new database.
/// </summary>
/// <param name="Token">New database token details.</param>
/// <param name="Name">Name of the database to be created.</param>
/// <param name="Definition">Database configurations.</param>
public sealed record NewDatabase(Token Token, Identifier Name, DatabaseDefinition Definition) : Statement(Token)
{
    public override string ToString()
    {
        return $"New Database ({Name}) {{ {Definition} }}";
    }
}

/// <summary>
///     DDL statement to delete an existing table.
/// </summary>
/// <param name="Token">Delete table token details.</param>
/// <param name="Name">Name of the table to be deleted.</param>
public sealed record DeleteTable(Token Token, Identifier Name) : Statement(Token)
{
    public override string ToString()
    {
        return $"Delete Table ({Name})";
    }
}

/// <summary>
///     DDL statement to create a new table.
/// </summary>
/// <param name="Token">New table token details.</param>
/// <param name="Name">Name of the table to be created.</param>
/// <param name="Definition">Table configurations.</param>
public sealed record NewTable(Token Token, Identifier Name, TableDefinition Definition) : Statement(Token)
{
    public override string ToString()
    {
        return $"New Table ({Name}) {{ {Definition} }}";
    }
}

/// <summary>
///     DDL statement to rename an existing table.
/// </summary>
/// <param name="Token">Rename table token details.</param>
/// <param name="OriginalName">Original name of the table to be renamed.</param>
/// <param name="NewName">New name of the table to be renamed.</param>
public sealed record RenameTable(Token Token, Identifier OriginalName, Identifier NewName) : Statement(Token)
{
    public override string ToString()
    {
        return $"Rename Table ({OriginalName}), ({NewName})";
    }
}