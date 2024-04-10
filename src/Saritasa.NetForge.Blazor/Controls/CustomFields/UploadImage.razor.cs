using CommunityToolkit.Mvvm.Messaging;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain.Entities.Options;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.Mvvm.ViewModels;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents upload image control.
/// </summary>
public partial class UploadImage : CustomField
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
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

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

    [Inject]
    private AdminOptions AdminOptions { get; init; } = null!;

    [Inject]
    private IFileService FileService { get; init; } = null!;

    [Inject]
    private ILogger<UploadImage> Logger { get; init; } = null!;

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

    private string? selectedBase64Image;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        selectedBase64Image = PropertyValue;
    }

    private async Task UploadImageAsync(IBrowserFile file)
    {
        error = null;

        selectedFile = file;

        try
        {
            // Convert to number of bytes.
            var maxImageSize = 1024 * 1024 * AdminOptions.MaxImageSizeInMb;
            var stream = file.OpenReadStream(maxImageSize);
            selectedFileBytes = await FileService.GetFileBytesAsync(stream, CancellationToken);
            selectedBase64Image =
                $"data:{selectedFile!.ContentType};base64,{Convert.ToBase64String(selectedFileBytes)}";

            PropertyValue = selectedBase64Image;

            WeakReferenceMessenger.Default.Unregister<UploadImageMessage>(this);
            WeakReferenceMessenger.Default.Register<UploadImage, UploadImageMessage>(this, (recipient, message) =>
            {
                message.Reply(recipient.ReceiveAsync());
            });
        }
        catch (IOException exception)
        {
            error = $"Uploaded file exceeds the maximum file size of {AdminOptions.MaxImageSizeInMb} MB.";

            Logger.LogInformation(
                exception,
                "Uploaded file exceeds the maximum file size of {MaxImageSize} MB.",
                AdminOptions.MaxImageSizeInMb);
        }
    }

    private void RemoveImage()
    {
        PropertyValue = null;
        selectedFile = null;
        selectedBase64Image = null;
    }

    /// <summary>
    /// Method to receive entity submit message.
    /// Used to commit an operation to an actual image on the storage.
    /// For example, create image only after submit updating of the entity.
    /// </summary>
    /// <remarks>
    /// For example create entity case: upload file, submit, create entity in database and create file.
    /// </remarks>
    private async Task ReceiveAsync()
    {
        if (selectedFile is not null)
        {
            PropertyValue = await Property.UploadFileStrategy!.UploadFileAsync(selectedFile, CancellationToken);
        }

        WeakReferenceMessenger.Default.Reset();
    }
}
