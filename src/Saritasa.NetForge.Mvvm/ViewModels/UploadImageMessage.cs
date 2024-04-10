using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Message about uploading image.
/// </summary>
public class UploadImageMessage : AsyncRequestMessage<Task>;
