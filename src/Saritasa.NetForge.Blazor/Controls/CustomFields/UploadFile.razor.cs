using Microsoft.AspNetCore.Components.Forms;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents upload file control.
/// </summary>
public partial class UploadFile : CustomField
{
    /// <summary>
    /// Property value.
    /// </summary>
    public byte[]? PropertyValue
    {
        get => (byte[]?)EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    private async Task UploadFileAsync(IBrowserFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        PropertyValue = memoryStream.ToArray();
    }
}
