using Microsoft.AspNetCore.Mvc;
using StronglyTypedIds;

namespace Saritasa.NetForge.Tests.Domain.Models;

/// <summary>
/// Represents a token entity with a strongly-typed primary key.
/// </summary>
internal class Token
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public TokenId Id { get; set; }

    /// <summary>
    /// Token name.
    /// </summary>
    public required string Name { get; set; }
}

/// <summary>
/// Identifier for <see cref="Token"/>.
/// </summary>
[StronglyTypedId(Template.Int)]
public readonly partial struct TokenId;
