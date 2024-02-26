using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Mvvm.ViewModels;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents upload file control.
/// </summary>
public partial class UploadFile : CustomField, IRecipient<EntitySubmittedMessage>, IDisposable
{
    private bool disposedValue;
    private readonly CancellationTokenSource cancellationTokenSource = new();

    /// <summary>
    /// Cancellation token.
    /// </summary>
    private CancellationToken CancellationToken => cancellationTokenSource.Token;

    /// <summary>
    /// Disposes resources of the view model.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    [Inject]
    private AdminOptions AdminOptions { get; init; } = null!;

    [Inject]
    private IFileService FileService { get; init; } = null!;

    [Inject]
    private ILogger<UploadFile> Logger { get; init; } = null!;

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

    private string? error;

    private async Task UploadFileAsync(IBrowserFile file)
    {
        error = null;

        selectedFile = file;

        try
        {
            // Convert to number of bytes.
            var maxImageSize = 1024 * 1024 * AdminOptions.MaxImageSizeInMb;
            var stream = file.OpenReadStream(maxImageSize);
            selectedFileBytes = await FileService.GetFileBytesAsync(stream, CancellationToken);

            PropertyValue = $"data:{selectedFile!.ContentType};base64,{Convert.ToBase64String(selectedFileBytes)}";

            if (Property.IsPathToImage)
            {
                WeakReferenceMessenger.Default.Register(this);
            }
        }
        catch (IOException exception)
        {
            error = $"Uploaded file exceeds the maximum file size of {AdminOptions.MaxImageSizeInMb} MB.";

            Logger.LogInformation(exception, "Uploaded file exceeds the maximum file size");
        }
    }

    private void RemoveImage()
    {
        PropertyValue = null;
        selectedFile = null;
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
        if (selectedFile is not null)
        {
            var filePath = Path.Combine(AdminOptions.MediaFolder, Property.ImageFolder, selectedFile!.Name);
            PropertyValue = filePath;
            var filePathToCreate = Path.Combine(AdminOptions.StaticFilesFolder, filePath);

            try
            {
                await FileService.CreateFileAsync(filePathToCreate, selectedFileBytes!, CancellationToken);
            }
            catch (Exception exception)
            {
                error = "Something went wrong with uploading the file.";
                message.HasErrors = true;
                PropertyValue = null;

                Logger.LogError(exception, "Error encountered when file was saved.");
            }
        }

        WeakReferenceMessenger.Default.Reset();
    }
}
