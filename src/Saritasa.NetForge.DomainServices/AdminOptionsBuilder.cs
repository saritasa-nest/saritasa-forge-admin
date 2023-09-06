using Saritasa.NetForge.Domain.Entities;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;

namespace Saritasa.NetForge.DomainServices;

/// <inheritdoc />
public class AdminOptionsBuilder : IAdminOptionsBuilder
{
    private AdminOptions Options { get; set; } = new();

    /// <inheritdoc />
    public IOrmOptionsBuilder? OrmOptionsBuilder { get; set; }

    /// <inheritdoc />
    public IAdminOptionsBuilder UseEndpoint(string url)
    {
        Options.AdminPanelEndpoint = url;
        return this;
    }

    /// <inheritdoc />
    public AdminOptions Create()
    {
        return Options;
    }
}
