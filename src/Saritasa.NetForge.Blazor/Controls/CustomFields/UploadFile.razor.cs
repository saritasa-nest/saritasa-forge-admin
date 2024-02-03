using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Mvvm.ViewModels;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents upload file control.
/// </summary>
public partial class UploadFile : CustomField, IRecipient<EntitySubmittedMessage>
{
    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    private IBrowserFile? selectedFile;

    private byte[]? selectedFileBytes;

    private async Task UploadFileAsync(IBrowserFile file)
    {
        selectedFile = file;

        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        selectedFileBytes = memoryStream.ToArray();

        if (Property.IsPathToImage)
        {
            WeakReferenceMessenger.Default.Register(this);

            var filePath = $"images/{file.Name}";
            PropertyValue = filePath;
            return;
        }

        PropertyValue = $"data:{selectedFile!.ContentType};base64,{Convert.ToBase64String(selectedFileBytes)}";
    }

    /// <summary>
    /// Method to receive entity submit message.
    /// Used to create file only after user confirmed intention to save changes to entity.
    /// </summary>
    /// <remarks>
    /// For example create entity case: upload file, submit, create entity in database and create file.
    /// </remarks>
    public async void Receive(EntitySubmittedMessage message)
    {
        if (Property.IsPathToImage)
        {
            var filePath = $"images/{selectedFile!.Name}";
            var filePathToCreate = $"wwwroot/{filePath}";

            await using var fileStream = File.Create(filePathToCreate);
            fileStream.Write(selectedFileBytes);
        }
    }
}
