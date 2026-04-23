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

[StronglyTypedId(Template.Int)]
public readonly partial struct TokenId;