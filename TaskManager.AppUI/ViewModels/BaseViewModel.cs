using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// base implementation for view models
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// occurs when a property value changes
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// raises the <see cref="PropertyChanged"/> event for the specified property
    /// </summary>
    /// <param name="propertyName">name of the changed property</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// sets the backing field and raises a property changed notification if the value was changed
    /// </summary>
    /// <typeparam name="T">type of the property value</typeparam>
    /// <param name="field">backing field reference</param>
    /// <param name="value">new val</param>
    /// <param name="propertyName">property name</param>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}