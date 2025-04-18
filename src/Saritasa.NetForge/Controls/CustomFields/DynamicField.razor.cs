using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Saritasa.NetForge.Domain.UseCases.Metadata.GetEntityById;
using Saritasa.NetForge.MVVM.ViewModels;

namespace Saritasa.NetForge.Controls.CustomFields;

/// <summary>
/// Represents field rendered dynamically depending on type of <see cref="Property"/>.
/// </summary>
public partial class DynamicField : ComponentBase
{
    /// <summary>
    /// Value of the field will be put in the property of this instance.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required object EntityInstance { get; set; }

    /// <summary>
    /// Field will be rendered for this property.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required PropertyMetadataDto Property { get; set; }

    /// <summary>
    /// Contains all error models for the form. It is required for adding error of the current field to this list.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required List<FieldErrorModel> FieldErrorModels { get; set; }

    /// <summary>
    /// In case of file field, uploading a file will be handled by this action.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public required Action<PropertyMetadataDto, IBrowserFile?> HandleFile { get; set; }

    /// <summary>
    /// If true, value of the field cannot be changed.
    /// </summary>
    [Parameter]
    public bool IsReadOnly { get; set; }

    private FieldErrorModel GetFieldError(PropertyMetadataDto property)
    {
        var fieldError = FieldErrorModels.FirstOrDefault(e => e.Property.Name == property.Name);
        if (fieldError is not null)
        {
            return fieldError;
        }

        fieldError = new FieldErrorModel
        {
            Property = property
        };
        FieldErrorModels.Add(fieldError);

        return fieldError;
    }

    private Dictionary<string, object> GetFieldParameters(PropertyMetadataDto property, FieldErrorModel fieldError)
    {
        var parameters = new Dictionary<string, object>
        {
            { nameof(CustomField.Property), property },
            { nameof(CustomField.EntityInstance), EntityInstance },
            { nameof(CustomField.IsReadOnly), IsReadOnly },
            { nameof(CustomField.FieldErrorModel), fieldError }
        };

        if (property.UploadFileStrategy is not null)
        {
            parameters.Add(nameof(UploadImage.OnFileSelected), HandleFile);
        }

        return parameters;
    }
}

