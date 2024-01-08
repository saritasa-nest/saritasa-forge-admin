namespace Saritasa.NetForge.Blazor.Infrastructure;

/// <summary>
/// State container.
/// </summary>
/// <remarks>
/// Use case: when we want to pass some object to another page without parameter.
/// </remarks>
public class StateContainer
{
    /// <summary>
    /// Value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// On state change.
    /// </summary>
    public event Action? OnStateChange;

    /// <summary>
    /// Set value.
    /// </summary>
    public void SetValue(object value)
    {
        Value = value;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChange?.Invoke();
    }
}
