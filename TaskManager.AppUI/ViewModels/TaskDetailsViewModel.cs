using System.Globalization;
using System.Windows.Input;
using TaskManager.AppUI.Services;
using TaskManager.AppUI.ViewModels;
using TaskManager.Repositories.Enums;
using TaskManager.Services.DTO;
using TaskManager.Services.Interfaces;

namespace TaskManager.AppUI.ViewModels;

/// <summary>
/// viewmodel for the task details page,
/// includes task display and management
/// </summary>
public sealed class TaskDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly ITaskService _taskService;
    private readonly INavigateService _navigationService;

    private int _taskId;
    private string _currentName = string.Empty;
    private string _currentDescription = string.Empty;
    private Priority _currentPriority;
    private DateTime _currentDueDate;
    private bool _currentIsFinished;

    private string _name = string.Empty;
    private string _priorityText = string.Empty;
    private string _dueText = string.Empty;
    private string _doneText = string.Empty;
    private string _overdueText = string.Empty;
    private string _description = string.Empty;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    public string PriorityText
    {
        get => _priorityText;
        set => SetProperty(ref _priorityText, value);
    }
    public string DueText
    {
        get => _dueText;
        set => SetProperty(ref _dueText, value);
    }
    public string DoneText
    {
        get => _doneText;
        set => SetProperty(ref _doneText, value);
    }
    public string OverdueText
    {
        get => _overdueText;
        set => SetProperty(ref _overdueText, value);
    }
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    public ICommand EditTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }

    /// <summary>
    /// initializes a new instance of the <see cref="TaskDetailsViewModel"/> class
    /// </summary>
    /// <param name="taskService">service used to load and modify task data</param>
    public TaskDetailsViewModel(
        ITaskService taskService,
        INavigateService navigationService)
    {
        _taskService = taskService;
        _navigationService = navigationService;

        EditTaskCommand = new Command(async () => await EditTaskAsync());
        DeleteTaskCommand = new Command(async () => await DeleteTaskAsync());
    }
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("taskId", out var value))
            return;

        if (!int.TryParse(value?.ToString(), out var taskId))
            return;

        _taskId = taskId;
    }

    public async Task InitializeAsync()
    {
        if (_taskId <= 0)
            return;

        await LoadTaskAsync();
    }

    private async Task LoadTaskAsync()
    {
        if (_taskId <= 0)
            return;

        await RunBusyAsync(LoadTaskCoreAsync);
    }

    private async Task LoadTaskCoreAsync()
    {
        var task = await _taskService.GetTaskDetailsAsync(_taskId);

        _currentName = task.Name;
        _currentDescription = task.Description;
        _currentPriority = task.Priority;
        _currentDueDate = task.DueDate;
        _currentIsFinished = task.IsFinished;

        Name = task.Name;
        PriorityText = $"Priority: {task.Priority}";
        DueText = $"Due: {task.DueDate:dd-MM-yyyy}";
        DoneText = $"Finished: {task.IsFinished}";
        OverdueText = $"Overdue: {task.IsOverdue}";
        Description = task.Description;
    }

    private async Task EditTaskAsync()
    {
        if (_taskId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var name = await Shell.Current.DisplayPromptAsync(
                    "Edit task",
                    "Task name:",
                    initialValue: _currentName,
                    maxLength: 100,
                    keyboard: Keyboard.Text);

                if (string.IsNullOrWhiteSpace(name))
                    return;

                var description = await Shell.Current.DisplayPromptAsync(
                    "Edit task",
                    "Description:",
                    initialValue: _currentDescription,
                    maxLength: 500,
                    keyboard: Keyboard.Text);

                if (description is null)
                    return;

                var priority = await PickPriorityAsync(_currentPriority);
                if (priority is null)
                    return;

                var dueDate = await PromptDateAsync(_currentDueDate);
                if (dueDate is null)
                    return;

                var isFinished = await Shell.Current.DisplayAlertAsync(
                    "Task status",
                    "Mark task as finished?",
                    "Yes",
                    "No");

                await _taskService.UpdateTaskAsync(_taskId, new UpdateTaskDto
                {
                    Name = name.Trim(),
                    Description = description.Trim(),
                    Priority = priority.Value,
                    DueDate = dueDate.Value,
                    IsFinished = isFinished
                });

                await LoadTaskCoreAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    private async Task DeleteTaskAsync()
    {
        if (_taskId <= 0)
            return;

        await RunBusyAsync(async () =>
        {
            try
            {
                var confirmed = await Shell.Current.DisplayAlertAsync(
                    "Delete task",
                    "Delete this task?",
                    "Delete",
                    "Cancel");

                if (!confirmed)
                    return;

                await _taskService.DeleteTaskAsync(_taskId);
                await _navigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", ex.Message, "OK");
            }
        });
    }

    private static async Task<Priority?> PickPriorityAsync(Priority currentValue)
    {
        var selected = await Shell.Current.DisplayActionSheetAsync(
            "Select priority",
            "Cancel",
            null,
            "Low",
            "Medium",
            "High",
            "Critical");

        if (selected == "Cancel")
            return currentValue;

        return selected switch
        {
            "Low" => Priority.Low,
            "Medium" => Priority.Medium,
            "High" => Priority.High,
            "Critical" => Priority.Critical,
            _ => currentValue
        };
    }

    private static async Task<DateTime?> PromptDateAsync(DateTime currentValue)
    {
        var input = await Shell.Current.DisplayPromptAsync(
            "Edit task",
            "Enter due date in format dd-MM-yyyy:",
            initialValue: currentValue.ToString("dd-MM-yyyy"),
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(input))
            return null;

        if (DateTime.TryParseExact(
                input.Trim(),
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var exactDate))
        {
            return exactDate;
        }

        if (DateTime.TryParse(input.Trim(), out var parsedDate))
        {
            return parsedDate;
        }

        await Shell.Current.DisplayAlertAsync("Invalid date", "Use format dd-MM-yyyy.", "OK");
        return null;
    }
}