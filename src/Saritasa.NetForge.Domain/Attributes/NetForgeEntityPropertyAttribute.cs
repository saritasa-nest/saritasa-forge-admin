namespace Saritasa.NetForge.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class NetForgeEntityPropertyAttribute : Attribute
{
    /// <summary>
    /// Whether the entity is hidden from the view.
    /// </summary>
    public bool IsHidden { get; set; }
}
