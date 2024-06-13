namespace Saritasa.NetForge.Mvvm.ViewModels;

/// <summary>
/// Validation entity view model.
/// </summary>
public abstract class ValidationEntityViewModel : BaseViewModel
{
    /// <summary>
    /// List of <see cref="FieldErrorModel"/> instances in the view model.
    /// </summary>
    public List<FieldErrorModel> FieldErrorModels { get; set; } = [];
}
