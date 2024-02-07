using Xunit;

namespace Saritasa.NetForge.Tests.Fixtures;

/// <summary>
/// Common tests constants.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// Constant for <see cref="CollectionDefinitionAttribute"/> value.
    /// </summary>
    internal const string DependencyInjection = "Dependency Injection";

    /// <summary>
    /// The name of the test cases orderer class.
    /// </summary>
    internal const string OrdererTypeName = "Xunit.Microsoft.DependencyInjection.TestsOrder.TestPriorityOrderer";

    /// <summary>
    /// The assembly for the test cases orderer.
    /// </summary>
    internal const string OrdererAssemblyName = "Xunit.Microsoft.DependencyInjection";
}
