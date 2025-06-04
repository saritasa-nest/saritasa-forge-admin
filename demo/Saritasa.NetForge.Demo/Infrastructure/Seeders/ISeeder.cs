namespace Saritasa.NetForge.Demo.Infrastructure.Seeders;

/// <summary>
/// Represent a data seeder.
/// </summary>
public interface ISeeder
{
    /// <summary>
    /// Seed.
    /// </summary>
    /// <param name="numberOfItems">Total items to create.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of created items.</returns>
    Task<int> Seed(int numberOfItems, CancellationToken cancellationToken = default);
}