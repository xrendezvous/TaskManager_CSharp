using TaskManager.Services.Interface;

namespace TaskManager.AppUI.ViewModels;

public sealed class TaskDetailsViewModel : BaseViewModel, IQueryAttributable
{
    private readonly ITaskService _taskService;

    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
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

    public TaskDetailsViewModel(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("taskId", out var value))
            return;

        if (!int.TryParse(value?.ToString(), out var taskId))
            return;

        LoadTask(taskId);
    }

    private void LoadTask(int taskId)
    {
        var task = _taskService.GetTaskDetails(taskId);

        Title = task.Title;
        PriorityText = $"Priority: {task.Priority}";
        DueText = $"Due: {task.DueDate:yyyy-MM-dd}";
        DoneText = $"Finished: {task.IsFinished}";
        OverdueText = $"Overdue: {task.IsOverdue}";
        Description = task.Description;
    }
}