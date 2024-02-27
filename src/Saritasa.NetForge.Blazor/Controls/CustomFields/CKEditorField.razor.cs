using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Saritasa.NetForge.Blazor.Controls.CustomFields;

/// <summary>
/// Represents CKEditor field.
/// </summary>
public partial class CKEditorField : CustomField, IAsyncDisposable
{
    private IJSObjectReference? jsModule;
    private ElementReference editor;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/NetForgeBlazor/Controls/CustomFields/CKEditorField.razor.js");
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
    public Task UpdateText(string editorText)
    {
        if (PropertyValue != editorText)
        {
            PropertyValue = editorText;
        }

        return Task.CompletedTask;
    }
}
