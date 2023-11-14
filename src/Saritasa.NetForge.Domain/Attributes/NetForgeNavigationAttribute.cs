namespace Saritasa.NetForge.Domain.Attributes;

/// <summary>
/// Represents an attribute used to provide metadata for the entity's navigation.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class NetForgeNavigationAttribute : NetForgePropertyAttributeBase
{
}
