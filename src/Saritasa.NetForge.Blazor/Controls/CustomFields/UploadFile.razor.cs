using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Mvvm.ViewModels;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents upload file control.
/// </summary>
public partial class UploadFile : CustomField, IRecipient<EntitySubmittedMessage>
{
    [Inject]
    private AdminOptions AdminOptions { get; init; } = null!;

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

    private string? pathToImageToDelete;

    private async Task UploadFileAsync(IBrowserFile file)
    {
        MarkPreviousImageToDeleteIfExists();

        selectedFile = file;

        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        selectedFileBytes = memoryStream.ToArray();

        PropertyValue = $"data:{selectedFile!.ContentType};base64,{Convert.ToBase64String(selectedFileBytes)}";

        if (Property.IsPathToImage)
        {
            RegisterEntitySubmittedMessage();
        }
    }

    private void MarkPreviousImageToDeleteIfExists()
    {
        if (PropertyValue is not null && selectedFile is null)
        {
            pathToImageToDelete = Path.Combine(AdminOptions.StaticFilesFolder, PropertyValue!);
        }
    }

    private void RemoveImage()
    {
        if (Property.IsPathToImage && selectedFile is null)
        {
            pathToImageToDelete = Path.Combine(AdminOptions.StaticFilesFolder, PropertyValue!);
            RegisterEntitySubmittedMessage();
        }

        PropertyValue = null;
        selectedFile = null;
    }

    private void RegisterEntitySubmittedMessage()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        WeakReferenceMessenger.Default.Register(this);
    }

    /// <summary>
    /// Method to receive entity submit message.
    /// Used to commit an operation to an actual image on the storage.
    /// For example, create image only after submit updating of the entity.
    /// </summary>
    /// <remarks>
    /// For example create entity case: upload file, submit, create entity in database and create file.
    /// </remarks>
    public async void Receive(EntitySubmittedMessage message)
    {
        if (pathToImageToDelete is not null)
        {
            File.Delete(pathToImageToDelete);
        }

        if (selectedFile is not null)
        {
            var imagePath = Path.Combine(AdminOptions.MediaFolder, Property.ImageFolder, selectedFile!.Name);
            var filePathToCreate = Path.Combine(AdminOptions.StaticFilesFolder, imagePath);

            Directory.CreateDirectory(Path.GetDirectoryName(filePathToCreate)!);

            await using var fileStream = File.Create(filePathToCreate);
            fileStream.Write(selectedFileBytes);

            PropertyValue = imagePath;
        }

        WeakReferenceMessenger.Default.Reset();
    }
}
