using System.ComponentModel.DataAnnotations;
using Saritasa.NetForge.MVVM.ViewModels;

namespace Saritasa.NetForge.MVVM.Utils;

/// <summary>
/// Error utility class for mapping validation errors.
/// </summary>
public static class ErrorMappingUtil
{
    /// <summary>
    /// Maps validation errors to the corresponding fields in the provided list of models.
    /// </summary>
    /// <param name="errorModels">The list of <see cref="FieldErrorModel"/> objects where the errors need to be mapped.</param>
    /// <param name="errors">The list of <see cref="ValidationResult"/> containing validation errors.</param>
    public static void MappingErrorToCorrectField(this List<FieldErrorModel> errorModels, List<ValidationResult> errors)
    {
        // Clear the error on the previous validation.
        errorModels.ForEach(e => e.ErrorMessage = string.Empty);

        foreach (var error in errors)
        {
            foreach (var member in error.MemberNames)
            {
                var errorModel = errorModels.FirstOrDefault(e => e.Property.PropertyPath == member);
                if (errorModel is null)
                {
                    continue;
                }

                errorModel.ErrorMessage = error.ErrorMessage!;
            }
        }
    }
}
