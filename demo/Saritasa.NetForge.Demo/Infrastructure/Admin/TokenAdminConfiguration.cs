using Saritasa.NetForge.Domain;
using Saritasa.NetForge.Domain.Interfaces;
using Saritasa.NetForge.Demo.Models;

namespace Saritasa.NetForge.Demo.Infrastructure.Admin;

/// <summary>
/// <see cref="Token"/> admin panel configuration.
/// </summary>
public class TokenAdminConfiguration : IEntityAdminConfiguration<Token>
{
    /// <inheritdoc />
    public void Configure(EntityOptionsBuilder<Token> entityOptionsBuilder)
    {
        // TokenId is a strongly-typed ID backed by int.
        // The TextField passes a string, so we need a custom converter to parse it into TokenId.
        entityOptionsBuilder.ConfigureProperty(token => token.Id, builder =>
        {
            builder.SetConverter(value => int.TryParse(value, out var id) ? new TokenId(id) : default);
        });
    }
}