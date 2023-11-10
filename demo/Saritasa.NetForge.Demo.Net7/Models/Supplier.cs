namespace Saritasa.NetForge.Demo.Net7.Models;

/// <summary>
/// Represents a supplier entity.
/// </summary>
public class Supplier
{
    /// <summary>
    /// Unique identifier for the supplier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the supplier.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Shops that this supplier works for.
    /// </summary>
    public List<Shop> Shops { get; set; } = new();
}
