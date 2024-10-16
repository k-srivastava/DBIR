namespace Transpiler;

/// <summary>
/// Configure conflict resolution options.
/// </summary>
public enum ConflictConfiguration
{
    Rollback,
    Abort,
    Fail,
    Ignore,
    Replace
}

/// <summary>
/// Configure key validation deference.
/// </summary>
public enum DeferConfiguration
{
    Deferred,
    Immediate
}

/// <summary>
/// Configure foreign key modification options.
/// </summary>
public enum ForeignKeyModifyConfiguration
{
    SetNull,
    SetDefault,
    Cascade,
    Restrict,
    NoAction
}

/// <summary>
/// Configure join options.
/// </summary>
public enum JoinConfiguration
{
    Inner,
    Left,
    Right,
    Full,
    Cross
}

/// <summary>
/// Create a new column.
/// </summary>
/// <param name="Type">Data type held by the column.</param>
/// <param name="Visible">Whether the column is visible or not.</param>
public readonly record struct ColumnDefinition(DataType Type, BooleanType Visible)
{
    public override string ToString()
    {
        return $"column-definition {{\n\tdata_type: {Type},\n\tvisible: {Visible},\n}}";
    }
}

/// <summary>
/// Create a new constraint.
/// </summary>
/// <param name="Column">Column on which the constraint operates.</param>
/// <param name="Unique">Conflict configuration for a unique constraint, if any.</param>
/// <param name="Check">Expression evaluated to validate constraint.</param>
/// <param name="Default">Default value for the column.</param>
/// <param name="PrimaryKey">Whether the constraint is a primary key, if any.</param>
/// <param name="ForeignKey">Whether the constraint is a foreign key, if any.</param>
public readonly record struct ConstraintDefinition(
    Identifier Column,
    ConflictConfiguration? Unique,
    Expression Check,
    DataType Default,
    PrimaryKeyConstraintDefinition? PrimaryKey,
    ForeignKeyConstraintDefinition? ForeignKey
)
{
    public override string ToString()
    {
        return
            $"constraint-definition {{\n\tcolumn: {Column},\n\tunique: {Unique},\n\tcheck: {Check},\n\t" +
            $"default: {Default},\n\tprimary-key: {PrimaryKey},\n\tforeign-key: {ForeignKey},\n}}";
    }
}

/// <summary>
/// Define a new database.
/// </summary>
/// <param name="Password">Password of the database.</param>
/// <param name="Encryption">Encryption used within the database.</param>
/// <param name="CharacterSet">Character set of the database.</param>
/// <param name="Timezone">Timezone supported within the database.</param>
/// <param name="CopyFrom">Whether the database copies from some other database, if any.</param>
public readonly record struct DatabaseDefinition(
    CharFieldType Password,
    BooleanType Encryption,
    CharFieldType CharacterSet,
    CharFieldType Timezone,
    Identifier? CopyFrom
)
{
    public override string ToString()
    {
        return
            $"database-definition {{\n\tpassword: {Password},\n\tencryption: {Encryption},\n\t" +
            $"character-set: {CharacterSet},\n\ttimezone: {Timezone},\n\tcopy-from: {CopyFrom},\n}}";
    }
}

/// <summary>
/// Delete a record.
/// </summary>
/// <param name="Where">Expression evaluated to find records to delete, if any.</param>
/// <param name="OrderBy">Order the deletion by names.</param>
/// <param name="Limit">Limit to which records are deleted.</param>
public readonly record struct DeleteDefinition(Expression? Where, IEnumerable<Identifier> OrderBy, UIntType Limit)
{
    public override string ToString()
    {
        return
            $"delete-definition {{\n\twhere: {Where},\n\torderBy: {string.Join(", ", OrderBy)},\n\tlimit: {Limit},\n}}";
    }
}

/// <summary>
/// Insert a record.
/// </summary>
/// <param name="Replace">Whether the record should replace an already existing one.</param>
public readonly record struct InsertDefinition(BooleanType Replace)
{
    public override string ToString()
    {
        return $"insert-definition {{\n\treplace: {Replace},\n}}";
    }
}

/// <summary>
/// Join two tables.
/// </summary>
/// <param name="JoinType">Type of join between the tables.</param>
/// <param name="On">Table to be joined on, if any.</param>
/// <param name="As">Name of the new joined table.</param>
public readonly record struct JoinDefinition(JoinConfiguration JoinType, Expression? On, Identifier As)
{
    public override string ToString()
    {
        return $"join-definition {{\n\tjoin-type: {JoinType},\n\ton: {On},\n\tas: {As},\n}}";
    }
}

/// <summary>
/// Select records.
/// </summary>
/// <param name="Distinct">Whether to select only distinct results.</param>
/// <param name="Where">Expression evaluated to find records to select, if any.</param>
/// <param name="GroupBy">Group the selected records by name.</param>
/// <param name="Having">Expression evaluated to group records to select, if any.</param>
/// <param name="OrderBy">Order the selected records by name.</param>
/// <param name="Limit">Limit to which records are selected.</param>
public readonly record struct SelectDefinition(
    BooleanType Distinct,
    Expression? Where,
    IEnumerable<Identifier> GroupBy,
    Expression? Having,
    IEnumerable<Identifier> OrderBy,
    UIntType Limit
)
{
    public override string ToString()
    {
        return
            $"select-definition {{\n\tdistinct: {Distinct},\n\twhere: {Where},\n\t" +
            $"group-by: {string.Join(", ", GroupBy)},\n\thaving: {Having},\n\t" +
            $"order-by: {string.Join(", ", OrderBy)},\n\tlimit: {Limit},\n}}";
    }
}

/// <summary>
/// Create a new table.
/// </summary>
/// <param name="Columns">Column details within the table.</param>
/// <param name="Constraints">Constraints within the table.</param>
public readonly record struct TableDefinition(
    IEnumerable<ColumnDefinition> Columns,
    IEnumerable<ConstraintDefinition> Constraints
)
{
    public override string ToString()
    {
        return
            $"table-definition {{\n\tcolumns: {string.Join(", ", Columns)},\n\t" +
            $"constraints: {string.Join(", ", Constraints)},\n}}";
    }
}

/// <summary>
/// Insert new records into a table.
/// </summary>
/// <param name="Mapping">Mapping of column names to values for the table.</param>
public readonly record struct TableInsertDefinition(Dictionary<Identifier, DataType> Mapping)
{
    public override string ToString()
    {
        return $"table-insert-definition {{\n\t{Mapping},\n}}";
    }
}

// TODO: Update definition.
public readonly record struct UpdateDefinition;

/// <summary>
/// Create a new primary key.
/// </summary>
/// <param name="Ascending">Whether to order in ascending order.</param>
/// <param name="AutoIncrement">Whether to automatically increment the next primary key.</param>
/// <param name="OnConflict">Conflict configuration system to use, if any.</param>
public readonly record struct PrimaryKeyConstraintDefinition(
    BooleanType Ascending,
    BooleanType AutoIncrement,
    ConflictConfiguration? OnConflict
)
{
    public override string ToString()
    {
        return
            $"primary-key-constraint-definition {{\n\tascending: {Ascending},\n\tauto-increment: {AutoIncrement},\n\t" +
            $"on-conflict: {OnConflict},\n}}";
    }
}

/// <summary>
/// Create a new foreign key.
/// </summary>
/// <param name="ReferenceColumns">Column referenced in the foreign key.</param>
/// <param name="OnUpdate">Modification configuration on key update, if any.</param>
/// <param name="OnDelete">Modification configuration on key delete, if any.</param>
/// <param name="Defer">Deference configuration on the new key, if any.</param>
public readonly record struct ForeignKeyConstraintDefinition(
    IEnumerable<Identifier> ReferenceColumns,
    ForeignKeyModifyConfiguration? OnUpdate,
    ForeignKeyModifyConfiguration? OnDelete,
    DeferConfiguration? Defer
)
{
    public override string ToString()
    {
        return
            $"foreign-key-constraint-definition {{\n\treference-columns: {string.Join(", ", ReferenceColumns)},\n\t" +
            $"on-update: {OnUpdate},\n\ton-delete: {OnDelete},\n\tdefer: {Defer},\n}}";
    }
}