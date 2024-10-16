namespace Transpiler;

/// <summary>
///     General expression node supported by DBIR.
/// </summary>
/// <param name="Token">Token corresponding to the expression.</param>
public abstract record Expression(Token Token)
{
    public abstract override string ToString();
}

/// <summary>
///     General identifier node supported by DBIR.
/// </summary>
/// <param name="Token">Token corresponding to the identifier.</param>
public record Identifier(Token Token) : Expression(Token)
{
    public override string ToString()
    {
        return Token.Lexeme;
    }
}