using StronglyTypedIds;

namespace Saritasa.NetForge.Demo.Models;

/// <summary>
/// Token.
/// </summary>
public class Token
{
    /// <summary>
    /// Id.
    /// </summary>
    public TokenId Id { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    public required string Name { get; set; }
}

/// <summary>
/// Identifier for <see cref="Token"/>.
/// </summary>
[StronglyTypedId(Template.Int)]
public readonly partial struct TokenId;