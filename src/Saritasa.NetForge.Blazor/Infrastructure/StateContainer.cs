namespace Saritasa.NetForge.Blazor.Infrastructure;

public class StateContainer
{
    public object Value { get; set; }

    public event Action OnStateChange;

    public void SetValue(object value)
    {
        Value = value;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        OnStateChange.Invoke();
    }
}
