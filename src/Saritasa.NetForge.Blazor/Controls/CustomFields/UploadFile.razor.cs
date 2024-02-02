using Microsoft.AspNetCore.Components;
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
    public object? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance);
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    [Parameter]
    public EventCallback ValueChanged { get; set; }

    private async Task UploadFileAsync(IBrowserFile file)
    {
        if (Property.IsImagePath)
        {
            var filePath = $"images/{file.Name}";
            var filePathToCreate = $"wwwroot/{filePath}";

            await using (var fileStream = File.Create(filePathToCreate))
            {
                using var memoryStream2 = new MemoryStream();
                await file.OpenReadStream().CopyToAsync(memoryStream2);

                fileStream.Write(memoryStream2.ToArray());
            }

            PropertyValue = filePath;
            return;
        }

        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        PropertyValue = memoryStream.ToArray();

        ValueChanged.
    }
}
