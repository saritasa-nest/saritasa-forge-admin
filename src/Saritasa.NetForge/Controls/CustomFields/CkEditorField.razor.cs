using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Represents CKEditor field.
/// </summary>
public partial class CkEditorField : CustomField, IAsyncDisposable
{
    private IJSObjectReference? jsModule;
    private ElementReference editor;

    /// <summary>
    /// JS runtime.
    /// </summary>
    [Inject]
    public IJSRuntime? JsRuntime { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            jsModule = await JsRuntime!.InvokeAsync<IJSObjectReference>("import",
                "./_content/Saritasa.NetForge/Controls/CustomFields/CkEditorField.razor.js");
            var editorId = Guid.NewGuid().ToString();
            var dotnetReference = DotNetObjectReference.Create(this);
            await jsModule.InvokeVoidAsync("InitCKEditor", editor, editorId, IsReadOnly,
                dotnetReference);
        }
    }

    /// <summary>
    /// Property value.
    /// </summary>
    public string? PropertyValue
    {
        get => EntityInstance.GetType().GetProperty(Property.Name)?.GetValue(EntityInstance)?.ToString();
        set => EntityInstance.GetType().GetProperty(Property.Name)?.SetValue(EntityInstance, value);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        if (jsModule is not null)
        {
            await jsModule.DisposeAsync();
        }
    }

    /// <summary>
    /// Updates the property content with the value from the editor.
    /// </summary>
    /// <param name="editorText">Text from the editor.</param>
    [JSInvokable]
    public async Task UpdateTextAsync(string editorText)
    {
        await Task.Run(() =>
        {
            if (PropertyValue != editorText)
            {
                PropertyValue = editorText;
            }
        });
    }
}
