using TaskManager.AppUI.ViewModels;
using TaskManager.Services.Interfaces;

/// <summary>
/// view model for the task details page
/// </summary>
public sealed class TaskDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly ITaskService _taskService;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _priorityText = string.Empty;
    public string PriorityText
    {
        get => _priorityText;
        set => SetProperty(ref _priorityText, value);
    }

    private string _dueText = string.Empty;
    public string DueText
    {
        get => _dueText;
        set => SetProperty(ref _dueText, value);
    }

    private string _doneText = string.Empty;
    public string DoneText
    {
        get => _doneText;
        set => SetProperty(ref _doneText, value);
    }

    private string _overdueText = string.Empty;
    public string OverdueText
    {
        get => _overdueText;
        set => SetProperty(ref _overdueText, value);
    }

    private string _description = string.Empty;
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    /// <summary>
    /// initializes a new instance of the <see cref="TaskDetailsViewModel"/> class
    /// </summary>
    /// <param name="taskService">service to load task data</param>
    public TaskDetailsViewModel(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// applies query parameters passed through Shell navigation
    /// </summary>
    /// <param name="query">dict containing navigation query parameters</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("taskId", out var value))
            return;

        if (!int.TryParse(value?.ToString(), out var taskId))
            return;

        LoadTask(taskId);
    }

    /// <summary>
    /// loads task details into the view model
    /// </summary>
    /// <param name="taskId">the task identifier</param>
    private void LoadTask(int taskId)
    {
        var task = _taskService.GetTaskDetails(taskId);

        Name = task.Name;
        PriorityText = $"Priority: {task.Priority}";
        DueText = $"Due: {task.DueDate:yyyy-MM-dd}";
        DoneText = $"Finished: {task.IsFinished}";
        OverdueText = $"Overdue: {task.IsOverdue}";
        Description = task.Description;
    }
}