using TaskManager.Services;

namespace TaskManager.AppUI;

[QueryProperty(nameof(TaskId), "taskId")]
public partial class TaskDetailsPage : ContentPage
{
    private readonly RepositoryService _repo;
    public string? TaskId { get; set; }

    public TaskDetailsPage(RepositoryService repo)
    {
        InitializeComponent();
        _repo = repo;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
        if (string.IsNullOrWhiteSpace(TaskId))
                return;

        int id = int.Parse(TaskId);

        var task = _repo.GetTask(id);

        TitleLabel.Text = task.Title;
        PriorityLabel.Text = $"Priority: {task.Priority}";
        DueLabel.Text = $"Due: {task.DueDate:yyyy-MM-dd}";
        DoneLabel.Text = $"Finished: {task.IsFinished}";
        OverdueLabel.Text = $"Overdue: {task.IsOverdue}";
        DescLabel.Text = task.Description;
    }
}