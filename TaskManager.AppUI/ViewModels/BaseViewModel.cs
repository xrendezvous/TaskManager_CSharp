using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// provides a base implementation for viewmodels with
/// busy-state mechanism for async
/// </summary>
public abstract class BaseViewModel : INotifyPropertyChanged
{
    private bool _isBusy;
    /// <summary>
    /// occurs when property value changes
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// gets/sets a value when viewmodel performs async operation
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
    }
    public bool IsNotBusy => !IsBusy;
    /// <summary>
    /// raises the <see cref="PropertyChanged"/> event for the 
    /// specified property
    /// </summary>
    /// <param name="propertyName">name of the property that has changed</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// sets backing field and raises a property changed notif if val was changed
    /// </summary>
    /// <typeparam name="T">type of the property val</typeparam>
    /// <param name="field">reference to the backing field</param>
    /// <param name="value">new val to assign</param>
    /// <param name="propertyName">name of the property being updated</param>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// runs the specified action while auto managing the busy state 
    /// of the viewmodel
    /// </summary>
    /// <param name="action">asynchronous action</param>
    /// <returns>a task representing the asynchronous run</returns>
    protected async Task RunBusyAsync(Func<Task> action)
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            await action();
        }
        finally
        {
            IsBusy = false;
        }
    }
}